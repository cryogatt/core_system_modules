using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerLevels;

namespace ContainerLevelsTest
{
    /// <summary>
    /// Test the containers secondary level type to ensure libary returns the correct level based on defined containers.
    /// </summary>
    [TestClass]
    public class SecondaryLevelContainersTest
    {
        /// <summary>
        /// Ensure Box is a secondary level container
        /// </summary>
        [TestMethod]
        public void BoxIsSecondaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.BOX_TYPE_ID) == ContainerLevelTypes.SECONDARY_CONTAINERS);
        }

        /// <summary>
        /// Ensure Rack is a secondary level container
        /// </summary>
        [TestMethod]
        public void RackIsSecondaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.RACK_TYPE_ID) == ContainerLevelTypes.SECONDARY_CONTAINERS);
        }

        /// <summary>
        /// Ensure Visotube is a secondary level container
        /// </summary>
        [TestMethod]
        public void VisotubeIsSecondaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.VISOTUBE_TYPE_ID) == ContainerLevelTypes.SECONDARY_CONTAINERS);
        }

        /// <summary>
        /// Ensure Goblet is a secondary level container
        /// </summary>
        [TestMethod]
        public void GobletIsSecondaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.GOBLET_TYPE_ID) == ContainerLevelTypes.SECONDARY_CONTAINERS);
        }

        /// <summary>
        /// Ensure Canister is a secondary level container
        /// </summary>
        [TestMethod]
        public void CanisterIsSecondaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.CANISTER_TYPE_ID) == ContainerLevelTypes.SECONDARY_CONTAINERS);
        }

        /// <summary>
        /// Ensure Cane is a secondary level container
        /// </summary>
        [TestMethod]
        public void CaneIsSecondaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.CANE_TYPE_ID) == ContainerLevelTypes.SECONDARY_CONTAINERS);
        }
    }
}


