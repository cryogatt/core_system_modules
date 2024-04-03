using System;
using System.Collections.Generic;
using ContainerLevels.ConcreteRules;

namespace ContainerLevels
{
    /// <summary>
    /// Purpose of rule is to return the level type (primary, secondary or root) of a given container.
    /// An enum of Container General Types is maintained for each level in each rule.
    /// </summary>
    public class RuleContainerLevelCalculator : IRuleContainerLevelCalculator
    {
        public RuleContainerLevelCalculator()
        {
            // Apply each rule
            Rules.Add(new RootLevelContainers());
            Rules.Add(new PrimaryContainers());
        }

        /// <summary>
        /// Returns the container type level if not found Secondary Container by deafult
        /// </summary>
        /// <returns></returns>
        public ContainerLevelTypes GetLevel(Int32 type)
        {
            // Loop through each rule, if more than one rules applies the last rule will be returned.
            foreach (var rule in Rules)
            {
                if (rule.DoesApply(type))
                {
                    return rule.GetLevel();
                }
            }
            
            return ContainerLevelTypes.SECONDARY_CONTAINERS;
        }

        private List<IContainerLevelRule> Rules = new List<IContainerLevelRule>();
    }
}