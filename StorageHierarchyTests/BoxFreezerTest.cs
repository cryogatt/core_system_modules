//using System;
//using System.Collections.Generic;
//using CryogattServerAPI.Models.Tables;
//using StorageHierarchy;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//// TODO Reimplement when architecture has been reconfigured to eliminate circular dependancy

//namespace StorageHierarchyTests
//{
//    [TestClass]
//    public class BoxFreezerTest
//    {
//        public BoxFreezerTest()
//        {
//            List = new List<Container>
//            {
//                Box,
//                Freezer
//            };

//            Calculator = new RuleStorageHierarchyCalculator(List); 
//        }

//        private Container Box = new Container
//        {
//            Id = 1,
//            Description = "Box",
//            ParentContainerStorageId = null
//        };
//        private Container Freezer = new Container
//        {
//            Id = 2,
//            Description = "Freezer",
//            ParentContainerStorageId = null
//        };

//        private List<Container> List;

//        RuleStorageHierarchyCalculator Calculator;

//        /// <summary>
//        /// Test applies storage rule and ensures that there is a rule for a box and freezer
//        /// </summary>
//        //[TestMethod]
//        //public void ApplyStorageRuleTest()
//        //{
//        //    Assert.AreNotEqual(Calculator.ApplyStorageRule(), null);
//        //}
//    }
//}
