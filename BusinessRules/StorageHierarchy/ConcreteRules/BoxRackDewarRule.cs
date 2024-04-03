using System.Collections.Generic;
using System.Linq;
using ContainerTypes;
using Infrastructure.Container.Entities;

namespace StorageHierarchy.ConcreteRules
{
    public class BoxRackDewarRule : IStorageRule
    {
        public BoxRackDewarRule(List<Container> list, IEnumerable<ContainerIdent> idents)
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
                types.Any(t => t == GeneralTypes.BOX_TYPE_ID) &&
                types.Any(t => t == GeneralTypes.RACK_TYPE_ID);
        }

        /// <summary>
        /// Sorts the container hierarchy based on specific rule 
        /// </summary>
        /// <returns></returns>
        public List<Container> OrganiseStorage()
        {
            AssignItems();

            // Update parent heirarchary 
            Rack.ParentContainerStorageId = Dewar.Id;
            Box.ParentContainerStorageId = Rack.Id;

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Dewar,
                Rack,
                Box
            };

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
            Box.ParentContainerStorageId = null;

            // Add updated items to new list
            List<Container> resp = new List<Container>
            {
                Dewar,
                Rack,
                Box
            };

            return resp;
        }

        private void AssignItems()
        {
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
        }

        private Container Dewar { get; set; }
        private Container Rack { get; set; }
        private Container Box { get; set; }

        private IEnumerable<ContainerIdent> Idents;

        private List<Container> List { get; set; }
    }
}