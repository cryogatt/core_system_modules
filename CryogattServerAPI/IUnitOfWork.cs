using ContainerLevels;
using Distribution.Services;
using Infrastructure.Container.Services;
using Infrastructure.History.Services;
using Infrastructure.Material.Services;
using Infrastructure.Users.Services;
using StorageHierarchy;
using StorageOperations;

namespace CryogattServerAPI
{
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Access to Container records.
        /// </summary>
        IContainerManager ContainerManager { get; }

        /// <summary>
        ///     Access to History records.
        /// </summary>
        IHistoryManager HistoryManager { get; }

        /// <summary>
        ///     Access to Material records.
        /// </summary>
        IMaterialManager MaterialManager { get; }

        /// <summary>
        ///     Access to user records.
        /// </summary>
        IUserManager UserManager { get; }

        /// <summary>
        ///     Access to the container level rules with respect to their type.
        /// </summary>
        IRuleContainerLevelCalculator RuleContainerLevelCalculator { get; }

        /// <summary>
        ///     Access to the storage heirarchy rules.
        /// </summary>
        IRuleStorageHierarchyCalculator RuleStorageHierarchyCalculator { get; }

        /// <summary>
        ///     Access to the storage manager for storage operations.
        /// </summary>
        IStorageOperationsManager StorageOperationsManager { get; }
        
        /// <summary>
        ///     Access to the shipping/order records.
        /// </summary>
        IDistributionManager DistributionManager { get;  }
    }
}