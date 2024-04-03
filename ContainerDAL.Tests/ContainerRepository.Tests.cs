using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Infrastructure.Container.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CommonEF;
using CommonEF.Services;
using TestData;

namespace ContainerDAL.Tests
{
    [TestClass]
    public class ContainerRepositoryTests
    {
        /// <summary>
        ///     Test subject.
        /// </summary>
        private ContainerRepository Service { get; set; }

        [TestInitialize]
        public void Arrange()
        {
            Service = new ContainerRepository(MockTestRepository.Factory.Object);
        }
             
        #region Container Test Methods

        /// <summary>
        ///     Containers can be retreived by uid.
        /// </summary>
        /// <param name="uid"></param>
        [DataTestMethod]
        [DataRow("Vial")]
        [DataRow("Box")]
        public void GetContainerTest(string uid)
        {
            // Act
            var container = Service.GetContainer(uid);
            // Assert
            Assert.AreEqual(uid, container.Uid);
        }

        /// <summary>
        ///     Ensure relationships can be retrieved 
        ///     by checking vial is a child of box.
        /// </summary>
        /// <param name="uid"></param>
        [DataTestMethod]
        [DataRow("Box")]
        public void GetContainerInlcudeRelationshipsTest(string uid)
        {
            // Act
            var container = Service.GetContainerInlcudeRelationships(uid);
            // Assert
            Assert.AreEqual(1, container.StorageChildren.Count());
        }

        /// <summary>
        ///     Ensure an ident can be retrieved by name.
        /// </summary>
        /// <param name="identName"></param>
        [DataTestMethod]
        [DataRow("Wheaton 2 ml vial")]
        [DataRow("Wheaton 10x10 Box")]
        public void GetIdentTest(string identName)
        {
            // Act
            var ident = Service.GetIdent(identName);
            // Assert
            Assert.IsNotNull(ident);
        }

        /// <summary>
        ///     Ensure an ident can be retrieved by its tag ident.
        /// </summary>
        /// <param name="tagIdent"></param>
        [DataTestMethod]
        [DataRow(10001)]
        [DataRow(65538)]
        public void GetIdentTest(int tagIdent)
        {
            // Act
            var ident = Service.GetIdent(tagIdent);
            // Assert
            Assert.IsNotNull(ident);
        }

        /// <summary>
        ///     Ensure the users site (container) can be retrieved.
        /// </summary>
        [TestMethod]
        public void GetUsersSiteContainer()
        {
            foreach (var user in MockTestRepository.UserData)
            {
                // Act
                var container = Service.GetUsersSiteContainer(user.Id);
                // Assert
                Assert.IsNotNull(container);
                Assert.IsNotNull(
                    MockTestRepository.SiteData
                    .Select(s => s.ContainerId == container.Id));
            }
        }

        #endregion
    }
}
