using System.Collections.Generic;
using System.Linq;
using ContainerTypes;
using Infrastructure.Container.Entities;

namespace StorageHierarchy.ConcreteRules
{
    public class VialBoxRackDewarRule : IStorageRule
    {
        public VialBoxRackDewarRule(List<Container> list, IEnumerable<ContainerIdent> idents)
        {
            Idents = idents;
            List = list;
        }

        /// <summary>
        ///     Determines if the list contains the relevant storage items for this particular rule.
        /// </summary>
        /// <returns></returns>
        public bool DoesApply(IEnumerable<int> types)
        {
            return
                types.Any(t => t == GeneralTypes.DEWAR_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.RACK_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.BOX_TYPE_ID) &&
                !types.Any(t => t == GeneralTypes.DRY_SHIPPER_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.VIAL_TYPE_ID);
        }

        /// <summary>
        ///     Sorts the container hierarchy based on specific rule 
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseStorage()
        {
            AssignItems();

            // Update parent heirarchary 
            Rack.ParentContainerStorageId = Dewar.Id;
            Box.ParentContainerStorageId = Rack.Id;

            // Update primary hierarachy 
            foreach (Container vial in Vials)
            {
                vial.ParentContainerStorageId = Box.Id;
            }

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Dewar,
                Rack,
                Box
            };
            resp.AddRange(Vials);

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
            Rack.ParentContainerStorageId = Dewar.Id;
            Box.ParentContainerStorageId = Rack.Id;

            // Update primary hierarachy 
            foreach (Container vial in Vials)
                vial.ParentContainerStorageId = null;

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Dewar,
                Rack,
                Box
            };
            resp.AddRange(Vials);

            return resp;
        }

        private void AssignItems()
        {
            // Grab parent storage items from list (assumes only one parent item for each parent item type, eg. 1 dewar, 1 rack, 1 box, etc). 
            Dewar = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.DEWAR_TYPE_ID)
                .SingleOrDefault().Id)
                .SingleOrDefault();

            Rack = List.Where(s => s.ContainerIdentId == Idents
                .Where(i => (i.TagIdent >> 16) == GeneralTypes.RACK_TYPE_ID)
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

        private Container Dewar { get; set; }
        private Container Rack { get; set; }
        private Container Box { get; set; }
        private List<Container> Vials { get; set; }

        private IEnumerable<ContainerIdent> Idents;

        private List<Container> List { get; set; }
    }
}