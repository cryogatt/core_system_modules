using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerTypes;

namespace ContainerTypesTest
{
    /// <summary>
    /// Purpose of tests are to ensure general types can be retreived from the ContainerTypes library.
    /// </summary>
    [TestClass]
    public class GeneralTypeTest
    {
        [TestMethod]
        public void VialTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.VIAL_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void BoxTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.BOX_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void RackTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.RACK_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void DewarTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.DEWAR_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void StrawTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.STRAW_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void VisotubeTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.VISOTUBE_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void GobletTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.GOBLET_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void CanisterTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.CANISTER_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void PotTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.POT_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void TestBenchTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.TEST_BENCH_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void StorageRackTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.STORAGE_RACK_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void CaneTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.CANE_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void FreezerTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.FREEZER_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void LocationTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.LOCATION_TYPE_ID)).Count != 0);
        }
        [TestMethod]
        public void DryShipperTypeExists()
        {
            Assert.IsTrue(ContainerIdentTypes.Types.FindAll(type => type.Ident.Equals(GeneralTypes.DRY_SHIPPER_TYPE_ID)).Count != 0);
        }
    }
}
