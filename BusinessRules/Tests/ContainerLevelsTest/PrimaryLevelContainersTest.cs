using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerLevels;

namespace ContainerLevelsTest
{
    /// <summary>
    /// Test the containers primary level type to ensure libary returns the correct level based on defined containers.
    /// </summary>
    [TestClass]
    public class PrimaryLevelContainersTest
    {
        /// <summary>
        /// Ensure Straw is a primary level container
        /// </summary>
        [TestMethod]
        public void StrawIsPrimaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.STRAW_TYPE_ID) == ContainerLevelTypes.PRIMARY_CONTAINER);
        }

        /// <summary>
        /// Ensure Vial is a primary level container
        /// </summary>
        [TestMethod]
        public void VialIsPrimaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.VIAL_TYPE_ID) == ContainerLevelTypes.PRIMARY_CONTAINER);
        }

        /// <summary>
        /// Ensure Pot is a primary level container
        /// </summary>
        [TestMethod]
        public void PotIsPrimaryLevelContainerTest()
        {
            Assert.IsTrue(new RuleContainerLevelCalculator().GetLevel(ContainerTypes.GeneralTypes.POT_TYPE_ID) == ContainerLevelTypes.PRIMARY_CONTAINER);
        }
    }
}

