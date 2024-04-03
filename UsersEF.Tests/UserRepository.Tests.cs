using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Infrastructure.Users.Entites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CommonEF;
using CommonEF.Services;
using TestData;

namespace UsersDAL.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        #region Private Methods
        
        /// <summary>
        ///     Test subject.
        /// </summary>
        private UserRepository Service { get; set; }

        #endregion

        #region Test startup

        [TestInitialize]
        public void Arrange()
        {
            Service = new UserRepository(MockTestRepository.Factory.Object);
        }

        #endregion

        #region User Test Methods

        [TestMethod]
        public void GetAllActiveUsersResponseBodiesTest()
        {
            var users = Service.GetAllActiveUsersResponseBodies();

            Assert.AreEqual(MockTestRepository.UserData.Count(), users.Count());
        }

        /// <summary>
        ///     User can be retreived by username.
        /// </summary>
        /// <param name="username"></param>
        [DataTestMethod]
        [DataRow("Alex R")]
        [DataRow("Alex D")]
        public void GetUserTest(string username)
        {
            // Act
            var user = Service.GetActiveUser(username);
            // Assert
            Assert.AreEqual(username, user.Username);
        }

        /// <summary>
        ///     All users can be retrieved and contains desired results.
        /// </summary>
        /// <param name="username"></param>
        [DataTestMethod]
        [DataRow("Alex R")]
        [DataRow("Alex D")]
        public void GetUsersTest(string username)
        {
            // Act
            var users = Service.GetAllActiveUsers();
            // Assert
            Assert.IsTrue(users.ToList().Any(u => u.Username == username));
        }

        #endregion

        #region Role Test Methods

        /// <summary>
        ///     Get user includes their associated role.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        [DataTestMethod]
        [DataRow("Alex R", "Admin")]
        [DataRow("Alex D", "StdUser")]
        public void GetUserIncludesRoleTest(string username, string roleName)
        {
            // Act
            var user = Service.GetActiveUser(username);
            // Assert
            Assert.AreEqual(roleName, user.Role.Name);
        }

        /// <summary>
        ///     Get all users includes the user role.
        /// </summary>
        [TestMethod]
        public void GetUsersIncludesRoleTest()
        {
            // Act
            var repsonse = Service.GetAllActiveUsers();
            // Assert
            Assert.IsTrue(repsonse
                .All(u => u.Role != null));
        }

        /// <summary>
        ///     All active users are returned
        /// </summary>
        /// <param name="username"></param>
        /// <param name="id"></param>
        [DataTestMethod]
        [DataRow("Alex R", 1)]
        [DataRow("Alex D", 2)]
        public void GetActiveUserResponseBodyTest(string username, int id)
        {
            // Act
            var response = Service.GetActiveUserResponseBody(id);
            // Assert
            Assert.AreEqual(username, response.Username);
        }

        #endregion

        #region Group Test Methods

        /// <summary>
        ///     Get user includes their list of groups.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="group"></param>
        [DataTestMethod]
        [DataRow("Alex R", "Group A")]
        [DataRow("Alex D", "Group B")]
        public void GetUserIncludesGroupTest(string username, string group)
        {
            // Act
            var user = Service.GetActiveUser(username);
            // Assert
            Assert.IsTrue(user.Groups.Any(g => g.Name == group));
        }

        /// <summary>
        ///     Get all users includes the user groups.
        /// </summary>
        [TestMethod]
        public void GetUsersIncludesGroupsTest()
        {
            // Act
            var repsonse = Service.GetAllActiveUsers();
            // Assert
            Assert.IsTrue(repsonse
                .All(u => u.Groups != null));
        }

        #endregion

        #region Site Tests

        /// <summary>
        ///     Site can be retieved by name.
        /// </summary>
        /// <param name="siteName"></param>
        [DataTestMethod]
        [DataRow("Nottingham")]
        public void GetSiteTest(string siteName)
        {
            // Act
            var response = Service.GetSite(siteName);
            // Assert
            Assert.AreEqual(siteName, response.Name);
        }

        /// <summary>
        ///         Get site records from database from their given names.
        /// </summary>
        [TestMethod]
        public void GetSitesTest()
        {
            // Act
            var response = Service.GetSites(MockTestRepository.SiteData.Select(s => s.Name).ToList());
            // Assert
            Assert.AreEqual(MockTestRepository.SiteData.Count(), response.Count());
        }
        #endregion
    }
}
