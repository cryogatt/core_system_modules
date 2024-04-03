using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerTypes;

namespace ContainerTypesTest
{
    [TestClass]
    // ReSharper disable once InconsistentNaming - Doesn't like RFID.
    public class GetRFIDEnabledContainerTests
    {
        /// <summary>
        /// Test assumes there is at least one RFID enabled container in the subtype list
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void GetGeneralRFIDEnabledContainersTest()
        {
            List<ContainerMake> generalTypes = ContainerIdentTypes.GetGeneralRFIDEnabledContainers();
            Assert.IsTrue(generalTypes.Count > 0);
        }

        /// <summary>
        /// Test assumes there is at least one non RFID enabled container in the subtype list
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void GetGeneralNonRFIDEnabledContainersTest()
        {
            List<ContainerMake> generalTypes = ContainerIdentTypes.GetGeneralNonRFIDEnabledContainers();
            Assert.IsTrue(generalTypes.Count > 0);
        }

        /// <summary>
        /// Test assumes there is at least one RFID enabled Vial in the subtype list
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void FindVialInGetGeneralRFIDEnbaledContainersTest()
        {
            List<ContainerMake> generalTypes = ContainerIdentTypes.GetGeneralRFIDEnabledContainers();
            Assert.IsTrue(generalTypes.FindAll(type => type.Ident.Equals(GeneralTypes.VIAL_TYPE_ID)).Count > 0);
        }

        /// <summary>
        /// Test assumes there is at least one RFID enabled container in the subtype list
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void GetSubtypeRFIDEnabledContainersTest()
        {
            List<ContainerModel> subtypes = ContainerIdentTypes.GetSubtypeRFIDEnabledContainers();
            Assert.IsTrue(subtypes.Count > 0);
        }

        /// <summary>
        /// Test assumes there is at least one non RFID enabled container in the subtype list
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void GetSubtypeNonRFIDEnabledContainersTest()
        {
            List<ContainerModel> subtypes = ContainerIdentTypes.GetSubtypeNonRFIDEnabledContainers();
            Assert.IsTrue(subtypes.Count > 0);
        }

        /// <summary>
        /// Test assumes Standard 2 ml Wheaton Vial exists in the list and is RFID Enabled
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void FindSpecificContainerInGetSubtypeRFIDEnabledContainersTest()
        {
            List<ContainerModel> subtypes = ContainerIdentTypes.GetSubtypeRFIDEnabledContainers();
            UInt32 modelIdent = subtypes.Find(sub => sub.Description.Equals("Wheaton 2 ml CryoElite vial")).TagIdent;
            Assert.AreEqual((UInt32)65538, modelIdent);
        }

 /// <summary>
        /// Test assumes Standard 2 ml Wheaton Vial exists in the list and is RFID Enabled
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void GetAllRFIDEnabledSubtypesOfGeneralType()
        {
            List<ContainerModel> subtypes = ContainerIdentTypes.GetSubtypeRFIDEnabledContainers(GeneralTypes.VIAL_TYPE_ID);
            Assert.IsTrue(subtypes.FindAll(sub => sub.Description.Equals("Wheaton 2 ml CryoElite vial")).Count > 0);
        }

        /// <summary>
        /// Test assumes Generic 11 stage rack exists in the list and is not RFID Enabled
        /// </summary>
        [TestMethod]
        // ReSharper disable once InconsistentNaming
        public void GetAllNonRFIDEnabledSubtypesOfGeneralType()
        {
            List<ContainerModel> subtypes = ContainerIdentTypes.GetSubtypeNonRFIDEnabledContainers(GeneralTypes.RACK_TYPE_ID);
            Assert.IsTrue(subtypes.FindAll(sub => sub.Description.Equals("Generic 11 stage rack")).Count > 0);
        }
    }
}
