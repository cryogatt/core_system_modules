using System.Collections.Generic;
using System.Linq;
using ContainerTypes;
using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;

namespace StorageHierarchy.ConcreteRules
{
    public class StrawOrVialCaneCanisterDewarRule : IStorageRule
    {
        public StrawOrVialCaneCanisterDewarRule(List<Container> list, IContainerManager containerManager, IEnumerable<ContainerIdent> idents)
        {
            Idents = idents;
            List = list;
            ContainerManager = containerManager;
        }

        /// <summary>
        /// Determines if the list contains the relevant storage items for this particular rule.
        /// </summary>
        /// <returns></returns>
        public bool DoesApply(IEnumerable<int> types)
        {
            return
                types.Any(t => t == GeneralTypes.DEWAR_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.CANISTER_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.CANE_TYPE_ID) &&
                !types.Any(t => t == GeneralTypes.DRY_SHIPPER_TYPE_ID) &&
                (types.Any(t => t == GeneralTypes.VIAL_TYPE_ID) || types.Any(t => t == GeneralTypes.STRAW_TYPE_ID));
        }

        /// <summary>
        /// Sorts the container hierarchy based on specific rule 
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseStorage()
        {
            AssignItems();

            // Update parent heirarchary 
            Canister.ParentContainerStorageId = Dewar.Id;
            Cane.ParentContainerStorageId = Canister.Id;

            // Update primary hierarachy 
            foreach (Container pri in PrimaryContainers)
            {
                pri.ParentContainerStorageId = Cane.Id;
            }

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Dewar,
                Canister,
                Cane
            };
            resp.AddRange(PrimaryContainers);

            return resp;
        }

        /// <summary>
        /// Determines which items are fit for withdrwal
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseWithdrawal()
        {
            // Set the storage items from the list
            AssignItems();

            // Update parent heirarchary 
            Canister.ParentContainerStorageId = Dewar.Id;

            // Determine if all primary containers are being withdrawn 
            var allWithinCane = ContainerManager.GetContainerContents(Cane.Uid);

            // Set the cane storage id to 0 if all items are withdrawn otherwise set to the canister
            if (allWithinCane != null)
            {
                Cane.ParentContainerStorageId = allWithinCane
                        .ToList()
                        .TrueForAll(c => 
                            PrimaryContainers.Any(pri => pri.Uid == c.Uid)) 
                            ? null 
                            : (int?)Canister.Id;

                if (Cane.ParentContainerStorageId == null)
                    Cane.StorageChildren = null;
            }

            // Update primary hierarachy 
            foreach (Container pri in PrimaryContainers)
            {
                pri.ParentContainerStorageId = null;
            }

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
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

        private Container Cane { get; set; }

        private List<Container> PrimaryContainers { get; set; }

        private IEnumerable<ContainerIdent> Idents;

        private List<Container> List { get; set; }

        private IContainerManager ContainerManager;
    }
}