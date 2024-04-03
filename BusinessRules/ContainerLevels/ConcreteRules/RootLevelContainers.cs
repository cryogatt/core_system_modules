// ReSharper disable InconsistentNaming
using System;

namespace ContainerLevels.ConcreteRules
{
    public class RootLevelContainers : IContainerLevelRule
    {
        /// <summary>
        /// Declaration of level
        /// </summary>
        private const ContainerLevelTypes Level = ContainerLevelTypes.ROOT_LEVEL_CONTAINER;
        
        /// <summary>
        /// List of general types that are root level containers 
        /// </summary>
        public enum GeneralTypes
        {
            DEWAR_TYPE_ID = 0x0004,
            FREEZER_TYPE_ID = 0x000d,
            LOCATION_TYPE_ID = 0x000e,
            DRY_SHIPPER_TYPE_ID = 0x000f
        }

        public RootLevelContainers()
        { }
        
        /// <summary>
        /// Determines if type is found in list
        /// </summary>
        /// <returns></returns>
        public bool DoesApply(Int32 type)
        {
            bool found = false;

            foreach (GeneralTypes val in Enum.GetValues(typeof(GeneralTypes)))
            {
                if (val == (GeneralTypes)type)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Returns the level of the rule 
        /// </summary>
        /// <returns></returns>
        public ContainerLevelTypes GetLevel() => Level;
    }
}