using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerTypes;

namespace ContainerTypesTest
{
    /// <summary>
    /// Purpose of tests are to ensure subtypes can retreived from the ContainerTypes library and their TagIdentId are correct. 
    /// </summary>
    [TestClass]
    public class SubtypeTest
    {
        /// <summary>
        /// Test assumes Standard 2 ml Wheaton Vial exists in the list and has the following description: Wheaton 2 ml CryoElite vial
        /// </summary>
        [TestMethod]
        public void StandardWheatonVialExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Subtypes.FindAll(subtype => subtype.Description.Equals("Wheaton 2 ml CryoElite vial")).Count > 0);
        }

        /// <summary>
        /// Test assumes Standard 2 ml Wheaton Vial exists in the list and has the following description: Wheaton 2 ml CryoElite vial
        /// </summary>
        [TestMethod]
        public void StandardWheatonVialHasCorrectIdent()
        {
            UInt32 modelIdent = ContainerIdentTypes.Subtypes.Find(subtype => subtype.Description.Equals("Wheaton 2 ml CryoElite vial")).TagIdent;
            Assert.AreEqual((UInt32)65538, modelIdent);
        }
    }
}
