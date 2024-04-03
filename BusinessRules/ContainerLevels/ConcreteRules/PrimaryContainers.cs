// ReSharper disable InconsistentNaming

using System;

namespace ContainerLevels.ConcreteRules
{
    public class PrimaryContainers : IContainerLevelRule
    {
        /// <summary>
        /// Declaration of level
        /// </summary>
        private const ContainerLevelTypes Level = ContainerLevelTypes.PRIMARY_CONTAINER;

        /// <summary>
        /// List of general types that are primary containers 
        /// </summary>
        public enum GeneralTypes
        {
            VIAL_TYPE_ID = 0x0001,
            STRAW_TYPE_ID = 0x0005,
            POT_TYPE_ID = 0x009
        }

        public PrimaryContainers() { }

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