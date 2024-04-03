using ContainerLevels;
using ContainerTypes;
using Distribution.Services;
using History.Entities;
using Infrastructure.Container.Services;
using Infrastructure.Distribution.Entities;
using Infrastructure.History.Services;
using Infrastructure.RFID.DTOs;
using Infrastructure.Users.Services;
using StorageHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageOperations
{
    public class StorageOperationsManager : IStorageOperationsManager
    {
        #region Constructors

        public StorageOperationsManager(IUserManager userManager, IContainerManager containerManager, 
            IHistoryManager historyManager, IRuleStorageHierarchyCalculator ruleStorageHierarchyCalculator,
            IDistributionManager distributionManager, IRuleContainerLevelCalculator ruleContainerLevelCalculator )
        {
            UserManager = userManager;
            ContainerManager = containerManager;
            HistoryManager = historyManager;
            RuleStorageHierarchyCalculator = ruleStorageHierarchyCalculator;
            DistributionManager = distributionManager;
            RuleContainerLevelCalculator = ruleContainerLevelCalculator;
        }

        #endregion

        #region Private Properties

        /// <summary>
        ///     Access to user data.
        /// </summary>
        private readonly IUserManager UserManager;

        /// <summary>
        ///     Access to container data.
        /// </summary>
        private readonly IContainerManager ContainerManager;

        /// <summary>
        ///     Access to history data.
        /// </summary>
        private readonly IHistoryManager HistoryManager;

        /// <summary>
        ///     Access to shipment data.
        /// </summary>
        private readonly IDistributionManager DistributionManager;

        /// <summary>
        ///     Access to rules regarding container level types (Vial = primary, etc).
        /// </summary>
        private readonly IRuleContainerLevelCalculator RuleContainerLevelCalculator;

        /// <summary>
        ///     Storage rules.
        /// </summary>
        private readonly IRuleStorageHierarchyCalculator RuleStorageHierarchyCalculator;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Process & persist the containers movements.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="storageItems"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        public void SetContainersMovement(StorageOperation operation, List<RFIDResponseBody> storageItems, int userId, string location)
        {
            var containers = ContainerManager.GetContainers(storageItems
                                                             .Select(s => s.Uid))
                                                             .ToList();

            // Is in db
            if (containers.Any(c => c == null))
                throw new Exception("Invalid list, one or more containers are not in database.");

            // Deep copy the value read from db
            var reference = containers.ConvertAll(c => c.Clone());

            // Set the new positions
            SetNewStorageIndexes(storageItems, containers);

            // Sort the Storage Hierarcy
            var reorganisedContainers =
                operation == StorageOperation.STORE || operation == StorageOperation.MOVEMENT
                ? RuleStorageHierarchyCalculator.ApplyStorageRule(containers, ContainerManager)
                : operation == StorageOperation.WITHDRAW ?
                RuleStorageHierarchyCalculator.ApplyWithdrawalRule(containers, ContainerManager) : null;

            if (reorganisedContainers == null)
                throw new ArgumentException("Storage configuration not supported!");

            // Gather list of all containers that have moved
            var movedContainers = reorganisedContainers
                .Where(c => !reference.Any(cont => cont.Equals(c)))
                .ToList();

            // Nothing to update
            if (movedContainers.Count() == 0)
                return; 

            // Update the movement
            if (operation == StorageOperation.STORE || operation == StorageOperation.MOVEMENT)
                ContainerManager.UpdateContainersBeingStored(movedContainers, userId, location);
            else if (operation == StorageOperation.WITHDRAW)
                ContainerManager.UpdateContainersBeingWithdrawn(movedContainers, userId, location);
            else
                throw new Exception("Storage operation not valid for method: Storage operation - " + operation);
        }

        /// <summary>
        ///     Apply rules to the shipment and contents.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="shipmentId"></param>
        /// <param name="tagUids"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        public void ProcessShipment(StorageOperation operation, int shipmentId, List<RFIDResponseBody> tagUids, int userId, string location)
        {
            var shipment = DistributionManager.GetShipment(shipmentId);
            if (shipment == null)
                throw new Exception("Shipment Id not valid: " + shipmentId);

            var containers = tagUids
                .Select(c => ContainerManager.GetContainer(c.Uid))
                .ToList();

            // Is in db
            if (containers.Any(c => c == null))
                throw new Exception("Invalid list, one or more containers are not in database.");

            // Deep copy the value read from db
            var reference = containers.ConvertAll(c => c.Clone());

            // Sort the Storage Hierarcy
            var reorganisedContainers =
                operation == StorageOperation.SEND
                ? RuleStorageHierarchyCalculator.ApplyStorageRule(containers, ContainerManager)
                : operation == StorageOperation.RECEIVE ?
                RuleStorageHierarchyCalculator.ApplyWithdrawalRule(containers, ContainerManager) : null;

            if (reorganisedContainers == null)
                throw new ArgumentException("Storage configuration not supported!");

            // Filter for primary containers only
            var primaryContainers = reorganisedContainers
                .Where(c =>
                    RuleContainerLevelCalculator.GetLevel(
                        ContainerManager.GetContainerIdent(c.ContainerIdentId).TagIdent >> 16)
                        == ContainerLevelTypes.PRIMARY_CONTAINER)
                .ToList();

            // Find the dry shipper
            var shipper = reorganisedContainers
                .Where(c => ContainerManager.GetContainerIdent(
                    c.ContainerIdentId).TagIdent >> 16
                    == GeneralTypes.DRY_SHIPPER_TYPE_ID)
                .Single();

            // Create couirer record if required
            var courier = ProcessCourier(shipment, shipper);

            // Validate items passed belong to shipment
            if (!ItemsBelongToShipment(shipment, primaryContainers))
                throw new ArgumentException($"Contents belongs to another shipment!");

            // Gather list of all containers that have moved
            var movedContainers = reorganisedContainers
                .Where(c => !reference.Any(cont => cont.Equals(c)))
                .ToList();

            var message = CreateAuditMessage(operation, shipment, shipper.Description);

            // Update record
            ContainerManager.UpdateContainersBeingShipped(movedContainers);
            ContainerManager.AuditContainers(
                primaryContainers,
                userId,
                location,
                message);

            UpdateContainerStatus(operation, userId, location, primaryContainers);

            // Update the shipment record
            if (operation == StorageOperation.SEND)
                DistributionManager.UpdateOrderStatus(shipment, 2);// TODO Remove magic numbers

            // Delete courier
            if (operation == StorageOperation.RECEIVE && ContainerManager.GetContainerContents(shipper.Uid) == null)
            {
                DistributionManager.DeleteCourier(courier);
                DistributionManager.UpdateOrderStatus(shipment, 3);
            }
        }

        private void UpdateContainerStatus(StorageOperation operation, int userId, string location, List<Infrastructure.Container.Entities.Container> primaryContainers)
        {
            var status = operation == StorageOperation.SEND
                ? "In Transit" :
                    operation == StorageOperation.RECEIVE
                    ? "Item Received" : null;

            var statuses = primaryContainers.Select(c => new ContainerStatus(0, c.Uid, status));
            HistoryManager.SetContainerStatus(statuses, location, userId);
        }

        private string CreateAuditMessage(StorageOperation operation, Shipment shipment, string shipperName)
        {
            var desination = UserManager.GetSite(shipment.RecipientId);

            var status = operation == StorageOperation.SEND
                ? "Dispatched" : "Arrived";

            var s = operation == StorageOperation.SEND
                ? "to" : "at";

            return $" Shipment: {shipment.ConsignmentNo} " +
                $"{status} " +
                $"in {shipperName} " +
                $"{s} {desination?.Name}";
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Ensure all items belong to shipmet
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="primaryContainers"></param>
        /// <returns></returns>
        private bool ItemsBelongToShipment(Shipment shipment, IEnumerable<Infrastructure.Container.Entities.Container> primaryContainers)
        {
            IEnumerable<Contents> shipmentContents = DistributionManager.GetShipmentContents(shipment.Id);

            foreach (var p in primaryContainers)
            {
                if (!shipmentContents.Any(c => c.ContainerId == p.Id))
                    throw new ArgumentException($"{p.Description} belongs to another shipment!");
            }

            return true;
        }

        /// <summary>
        ///     Creates a new courier record if required and checks if courier is already in use.
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="dryShipper"></param>
        /// <returns></returns>
        private Courier ProcessCourier(Shipment shipment, Infrastructure.Container.Entities.Container dryShipper)
        {
            var courier = DistributionManager.GetCourier(dryShipper.Id);
            // Add if not already in db
            if (courier == null)
                return CreateShipmentCourier(shipment, dryShipper);

            // Record already exists
            if (courier.ShipmentId == shipment.Id)
                return courier;

            // Courier exists & is assigned to another shipment
            // Has the shipment been forgotten about?
            if (ContainerManager.GetContainerContents(dryShipper.Uid).Count() != 0)
                throw new ArgumentException($"{dryShipper.Description} assigned to another shipment!");
            else
            {
                // Courier record must have not been removed from last shipment
                DistributionManager.DeleteCourier(courier);
                return CreateShipmentCourier(shipment, dryShipper);
            }
        }

        private Courier CreateShipmentCourier(Shipment shipment, Infrastructure.Container.Entities.Container shipper)
        {
            var courier = new Courier
            {
                ShipmentId = shipment.Id,
                ContainerId = shipper.Id
            };

            DistributionManager.AddCourier(courier);

            return courier;
        }

        /// <summary>
        ///     Set the new storage positions.
        /// </summary>
        /// <param name="NewRecords"></param>
        /// <param name="OldRecords"></param>
        private static void SetNewStorageIndexes(List<RFIDResponseBody> NewRecords, List<Infrastructure.Container.Entities.Container> OldRecords)
        {
            // Set the (possibly) new storage index
            OldRecords.ForEach(cont => cont.StorageIndex = (from item in NewRecords where item.Uid.Equals(cont.Uid) select item.Position).FirstOrDefault());
        }

        #endregion
    }
}
