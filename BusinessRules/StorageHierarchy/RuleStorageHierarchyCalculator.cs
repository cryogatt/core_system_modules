using System.Collections.Generic;
using System.Linq;
using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;
using StorageHierarchy.ConcreteRules;

namespace StorageHierarchy
{
    public class RuleStorageHierarchyCalculator : IRuleStorageHierarchyCalculator
    {
        private void Init(List<Container> list, IContainerManager containerManager, IEnumerable<ContainerIdent> idents)
        {
            List = list;
            ContainerManager = containerManager;

            // Add new rules here, the order will become important if more than one rule applies.
            Rules.Add(new VialBoxRackDewarRule(List, idents));
            Rules.Add(new StrawOrVialCaneCanisterDewarRule(List, ContainerManager, idents));
            Rules.Add(new StrawOrVialCaneShipperRule(List, idents));
            Rules.Add(new VialBoxFreezerRule(list, ContainerManager, idents));
            Rules.Add(new BoxFreezerRule(list, idents));
            Rules.Add(new BoxShipperRule(list, idents));
            Rules.Add(new VialBoxRule(List, ContainerManager, idents));
            Rules.Add(new BoxRackDewarRule(list, idents));
            Rules.Add(new StrawOrVialCaneCanisterShipmentRule(list, idents));
        }

        /// <summary>
        /// Configures storage heirarchy as per the storage rule that applies to the type of containers in the list
        /// </summary>
        /// list of containers
        /// <returns>null if list passed does not apply to known rules</returns>
        public List<Container> ApplyStorageRule(List<Container> list, IContainerManager containerManager)
        {           
            var containerIdents = containerManager.GetContainerIdents(list.Select(c => c.ContainerIdentId));

            Init(list, containerManager, containerIdents);

            return Rules
                .Where(rule => rule.DoesApply(containerIdents.Select(i => i.ContainerTypeId)))
                .FirstOrDefault()
                ?.OrganiseStorage();
        }

        /// <summary>
        /// Configures storage heirarchy as per the withdrawal rule that applies to the type of containers in the list
        /// </summary>
        /// list of containers
        /// <returns>null if list passed does not apply to known rules</returns>
        public List<Container> ApplyWithdrawalRule(List<Container> list, IContainerManager containerManager)
        {
            var containerIdents = containerManager.GetContainerIdents(list.Select(c => c.ContainerIdentId));

            Init(list, containerManager, containerIdents);

            return Rules
                .Where(rule => rule.DoesApply(containerIdents.Select(i => i.ContainerTypeId)))
                .FirstOrDefault()
                ?.OrganiseWithdrawal();
        }

        private List<IStorageRule> Rules = new List<IStorageRule>();
        private List<Container> List { get; set; }

        private IContainerManager ContainerManager;
    }
}