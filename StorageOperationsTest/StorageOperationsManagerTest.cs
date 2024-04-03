using System;
using ContainerLevels;
using Distribution.Services;
using Infrastructure.Container.Services;
using Infrastructure.History.Services;
using Infrastructure.Users.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StorageHierarchy;
using StorageOperations;

namespace StorageOperationsTest
{
    [TestClass]
    public class StorageOperationsManagerTest
    {
        /// <summary>
        ///     Test subject.
        /// </summary>
        private StorageOperationsManager StorageOperationsManager { get; set; }

        /// <summary>
        ///     Mock representation of user business logic.
        /// </summary>
        private Mock<IUserManager> MockUserManager { get; set; }

        /// <summary>
        ///     Mock representation of container business logic.
        /// </summary>
        private Mock<IContainerManager> MockContainerManager { get; set; }

        /// <summary>
        ///     Mock representation of History business logic.
        /// </summary>
        private Mock<IHistoryManager> MockHistoryManager { get; set; }

        /// <summary>
        ///     Mock representation of distribution business logic.
        /// </summary>
        private Mock<IDistributionManager> MockDistributionManager { get; set; }

        /// <summary>
        ///     Mock represention of the storage hierarchy rules.
        /// </summary>
        private Mock<IRuleStorageHierarchyCalculator> MockRuleStorageHierarchyCalculator { get; set; }

        /// <summary>
        ///     Mock represention of the storage hierarchy rules.
        /// </summary>
        private Mock<IRuleContainerLevelCalculator> MockRuleContainerLevelCalculator { get; set; }

    }
}
