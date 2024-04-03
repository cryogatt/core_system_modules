using System.Collections.Generic;
using System.Linq;
using ContainerService;
using ContainerTypes;
using Infrastructure.Container.Entities;

namespace StorageHierarchy.ConcreteRules
{

    public class StrawOrVialCaneShipperRule : IStorageRule
    {
        public StrawOrVialCaneShipperRule(List<Container> list, IEnumerable<ContainerIdent> idents)
        {
            Idents = idents;
            List = list;
        }

        /// <summary>
        /// Determines if the list contains the relevant storage items for this particular rule.
        /// </summary>
        /// <returns></returns>
        public bool DoesApply(IEnumerable<int> types)
        {
            return
                types.Any(t => t == GeneralTypes.DEWAR_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.DRY_SHIPPER_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.CANISTER_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.CANE_TYPE_ID) &&
                (types.Any(t => t == GeneralTypes.VIAL_TYPE_ID) || types.Any(t => t == GeneralTypes.STRAW_TYPE_ID));
        }

        /// <summary>
        /// From the shipments perpective storage is the sending of goods - i.e items are stored in the shipper 
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseStorage()
        {
            // Set the storage items from the list
            AssignItems();

            // Update parent heirarchary 
            Shipper.ParentContainerStorageId = null;
            Cane.ParentContainerStorageId = Shipper.Id;
            Canister.ParentContainerStorageId = Dewar.Id;

            // Update primary hierarachy 
            foreach (Container pri in PrimaryContainers)
            {
                pri.ParentContainerStorageId = Cane.Id;
            }

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Shipper,
                Dewar,
                Canister,
                Cane
            };
            resp.AddRange(PrimaryContainers);

            return resp;
        }

        /// <summary>
        /// From the shipments perpective withdraw is the recieving of goods - i.e items are withdrawn from the shipper
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseWithdrawal()
        {
            // Set the storage items from the list
            AssignItems();

            // Update parent heirarchary 
            Shipper.ParentContainerStorageId = Dewar.ParentContainerStorageId;
            Canister.ParentContainerStorageId = Dewar.Id;
            Cane.ParentContainerStorageId = Canister.Id;

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Shipper,
                Dewar,
                Canister,
                Cane
            };
            resp.AddRange(PrimaryContainers);

            return resp;
        }

        private void AssignItems()
        {
            Dewar = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.DEWAR_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            Shipper = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.DRY_SHIPPER_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            Canister = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.CANISTER_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            Cane = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.CANE_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            // Grab primary storage items
            PrimaryContainers = new List<Container>();

            PrimaryContainers.AddRange(List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.VIAL_TYPE_ID)
                .FirstOrDefault()?.Id)
                .ToList());

            PrimaryContainers.AddRange(List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.STRAW_TYPE_ID)
                .FirstOrDefault()?.Id)
                .ToList());
        }

        private Container Dewar { get; set; }
        private Container Canister { get; set; }
        private Container Shipper { get; set; }
        private Container Cane { get; set; }
        private List<Container> PrimaryContainers { get; set; }

        private IEnumerable<ContainerIdent> Idents;

        private List<Container> List { get; set; }

        private ContainerManager ContainerManager;
    }
}