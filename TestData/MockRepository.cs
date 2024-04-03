using CommonEF;
using CommonEF.Services;
using Infrastructure.Container.Entities;
using Infrastructure.Users.Entites;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData
{
    public static class MockTestRepository
    {
        static MockTestRepository()
        {
            // Insert data
            SetTestData();

            // Mock return values
            MockReturnValues();

            // Set factory
            Factory = new Mock<IContextFactory>();
            Factory.Setup(r => r.Create()).Returns(MockContext.Object);
        }

         /// <summary>
        ///     Mock database context.
        /// </summary>
        private static Mock<ICryogatt> MockContext { get; set; }

        /// <summary>
        ///     Mock context factory.
        /// </summary>
        public static Mock<IContextFactory> Factory { get; private set; }

        #region Mock DbSets

        /// <summary>
        ///     Mock representation of users.
        /// </summary>
        public static Mock<DbSet<User>> MockUsersSet { get; private set; }

        /// <summary>
        ///     Mock representation of users.
        /// </summary>
        public static Mock<DbSet<Group>> MockGroupSet { get; private set; }

        /// <summary>
        ///     Mock representation of Roles.
        /// </summary>
        public static Mock<DbSet<Role>> MockRoleSet { get; private set; }

        /// <summary>
        ///     Mock representation of Sites.
        /// </summary>
        public static Mock<DbSet<Site>> MockSiteSet { get; private set; }

        /// <summary>
        ///     Mock representation of containers.
        /// </summary>
        public static Mock<DbSet<Container>> MockContainerSet { get; private set; }

        /// <summary>
        ///     Mock representation of Idents.
        /// </summary>
        public static Mock<DbSet<ContainerIdent>> MockIdentSet { get; private set; }

        #endregion

        #region Test Data

        /// <summary>
        ///     Dummy Containers for tests.
        /// </summary>
        public static IQueryable<Container> ContainerData { get; private set; }

        /// <summary>
        ///     Dummy Idents for tests.
        /// </summary>
        public static IQueryable<ContainerIdent> IdentsData { get; private set; }

        /// <summary>
        ///     Dummy users for tests.
        /// </summary>
        public static IQueryable<User> UserData { get; private set; }

        /// <summary>
        ///     Dummy groups for tests.
        /// </summary>
        public static IQueryable<Group> GroupData { get; private set; }

        /// <summary>
        ///     Dummy roles for tests.
        /// </summary>
        public static IQueryable<Role> RoleData { get; private set; }

        /// <summary>
        ///     Dummy sites for tests.
        /// </summary>
        public static IQueryable<Site> SiteData { get; private set; }

        #endregion

        private static void MockReturnValues()
        {
            MockContainerSet = SetDataSetReturnValues(ContainerData);
            MockIdentSet = SetDataSetReturnValues(IdentsData);
            MockUsersSet = SetDataSetReturnValues(UserData);
            MockGroupSet = SetDataSetReturnValues(GroupData);
            MockRoleSet = SetDataSetReturnValues(RoleData);
            MockSiteSet = SetDataSetReturnValues(SiteData);

            MockContext = new Mock<ICryogatt>();
            MockContext.Setup(c => c.Containers).Returns(MockContainerSet.Object);
            MockContext.Setup(c => c.ContainerIdents).Returns(MockIdentSet.Object);
            MockContext.Setup(u => u.Roles).Returns(MockRoleSet.Object);
            MockContext.Setup(u => u.Sites).Returns(MockSiteSet.Object);
            MockContext.Setup(u => u.Groups).Returns(MockGroupSet.Object);
            MockContext.Setup(u => u.Users).Returns(MockUsersSet.Object);
        }

        private static void SetTestData()
        {
            RoleData = new List<Role>
            {
                new Role("Admin", "Super user of the system", DateTime.Now),
                new Role("StdUser", "Standard user of the system", DateTime.Now)
            }.AsQueryable();

            GroupData = new List<Group>
            {
                new Group("Group A", "First Group in repository", DateTime.Now),
                new Group("Group B", "Second Group in repository", DateTime.Now)
            }.AsQueryable();

            SiteData = new List<Site>
            {
                new Site(1, "Nottingham", "Nottingham", 5),
            }.AsQueryable();

            UserData = new List<User>
            {
                new User(1, "Alex", "Ricketts", "Alex R", "alex.ricketts@cryogatt.com", "password", 1, SiteData.ToList(),
                GroupData.ToList(), null, null, DateTime.Now, RoleData.Where(r => r.Name == "Admin").First()),

                new User(2, "Alex", "Durrant", "Alex D", "alex.Durrant@cryogatt.com", "password", 2, SiteData.ToList(),
                GroupData.ToList(), null, null, DateTime.Now, RoleData.Where(r => r.Name == "StdUser").First())
            }.AsQueryable();

            IdentsData = new List<ContainerIdent>
            {
                new ContainerIdent(1, "Wheaton 2 ml vial", 65538, 1),
                new ContainerIdent(2, "Wheaton 10x10 Box", 131073, 2),
                new ContainerIdent(3, "Generic 10 Stage Rack", 196611, 3),
                new ContainerIdent(4, "Statebourne Bio 12", 262164, 4),
                new ContainerIdent(1, "Site", 917506, 14)
            }.AsQueryable();

            var vial = new Container(1, "Vial", "vial 1", 1, 2, 1, null, 0, 0, DateTime.Now, null);
            var box = new Container(2, "Box", "box 1", 2, 3, 5, null, 0, 0, DateTime.Now, new List<Container> { vial });
            var Stack = new Container(3, "Stack", "Stack 1", 3, 4, 0, null, 0, 0, DateTime.Now, new List<Container> { box });
            var Dewar = new Container(4, "Dewar", "Dewar 1", 4, 5, 0, null, 0, 0, DateTime.Now, new List<Container> { Stack });
            var Site = new Container(5, "Site", "Site 1", 5, null, 0, null, 0, 0, DateTime.Now, new List<Container> { Dewar });

            ContainerData = new List<Container>
            {
                vial, box, Stack, Dewar, Site
            }.AsQueryable();
        }

        private static Mock<DbSet<T>> SetDataSetReturnValues<T>(IQueryable<T> returnData) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(returnData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(returnData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(returnData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(returnData.GetEnumerator());

            return mockSet;
        }
    }
}
