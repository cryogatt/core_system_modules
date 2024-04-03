using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerLevels;

namespace ContainerLevelsTest
{
    /// <summary>
    /// Test the containers Root level type to ensure libary returns the correct level based on defined containers.
    /// </summary>
    [TestClass]
    public class RootLevelContainersTest
    {
        /// <summary>
        /// Ensure Dewar is a root level container
        /// </summary>
        [TestMethod]
        public void DewarIsRootLevelContainersTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.DEWAR_TYPE_ID) == ContainerLevelTypes.ROOT_LEVEL_CONTAINER);
        }

        /// <summary>
        /// Ensure Shipper is a root level container
        /// </summary>
        [TestMethod]
        public void ShipperIsRootLevelContainersTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.DRY_SHIPPER_TYPE_ID) == ContainerLevelTypes.ROOT_LEVEL_CONTAINER);
        }

        /// <summary>
        /// Ensure Freezer is a root level container
        /// </summary>
        [TestMethod]
        public void FreezerIsRootLevelContainersTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.FREEZER_TYPE_ID) == ContainerLevelTypes.ROOT_LEVEL_CONTAINER);
        }

        /// <summary>
        /// Ensure Freezer is a root level container
        /// </summary>
        [TestMethod]
        public void LocationIsRootLevelContainersTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.LOCATION_TYPE_ID) == ContainerLevelTypes.ROOT_LEVEL_CONTAINER);
        }
    }
}
