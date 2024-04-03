using System.Collections.Generic;
using System.Linq;
using ContainerTypes;
using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;

namespace StorageHierarchy.ConcreteRules
{
    public class VialBoxFreezerRule : IStorageRule
    {
        public VialBoxFreezerRule(List<Container> list, IContainerManager containerManager, IEnumerable<ContainerIdent> idents)
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
                types.Any(t => t == GeneralTypes.FREEZER_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.BOX_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.VIAL_TYPE_ID);
        }

        /// <summary>
        /// Sorts the container hierarchy based on specific rule 
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseStorage()
        {
            AssignItems();

            // Update parent heirarchary 
            Box.ParentContainerStorageId = Freezer.Id;

            // List of vials already in the positions of the new entries
            List<Container> containersWithdrawn = new List<Container>();

            // Update primary hierarachy 
            foreach (Container vial in Vials)
            {
                // If not stored in a position - or at all
                if (vial.StorageIndex == 0)
                    continue;
                
                // Get the container in that is recorded in that position
                Container containerInPosn = ContainerManager.GetContainerByStorageIndex(Box.Id, vial.StorageIndex);

                // Is anything recorded in that place?
                if (containerInPosn == null)
                    continue;

                // Is the vial recorded in that position the same as in list?
                if (containerInPosn.Uid == vial.Uid)
                    continue;

                // If vial that is missing in not in the list to be placed in another positon
                if (!Vials.Any(c => c.Uid == containerInPosn.Uid))
                {
                    // Set the item to withdrawn
                    containerInPosn.ParentContainerStorageId = null;
                    containerInPosn.StorageIndex = 0;
                    containersWithdrawn.Add(containerInPosn);
                }                
                
                vial.ParentContainerStorageId = Box.Id;
            }

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Freezer,
                Box
            };
            resp.AddRange(Vials);
            
            if (containersWithdrawn.Count > 0)
            {
                resp.AddRange(containersWithdrawn);
            }
            
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
            Box.ParentContainerStorageId = Freezer.Id;

            // Update primary hierarachy 
            foreach (Container vial in Vials)
            {
                vial.ParentContainerStorageId = null;
                vial.StorageIndex = 0;
            }

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Freezer,
                Box
            };
            resp.AddRange(Vials);

            return resp;
        }

        private void AssignItems()
        {
            Freezer = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.FREEZER_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            Box = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.BOX_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            // Grab primary storage items
            Vials = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.VIAL_TYPE_ID)
                .FirstOrDefault().Id)
                .ToList();
        }

        private Container Freezer { get; set; }

        private Container Box { get; set; }

        private List<Container> Vials { get; set; }

        private IEnumerable<ContainerIdent> Idents;

        private List<Container> List { get; set; }

        private IContainerManager ContainerManager;
    }
}