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
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IMaterialManager materialManager, IUserManager userManager, IContainerManager containerManager, 
            IHistoryManager historyManager, IRuleContainerLevelCalculator ruleContainerLevelCalculator, 
            IRuleStorageHierarchyCalculator ruleStorageHierarchyCalculator, IStorageOperationsManager storageOperationsManager,
            IDistributionManager distributionManager)
        {
            MaterialManager = materialManager;
            UserManager = userManager;
            ContainerManager = containerManager;
            HistoryManager = historyManager;
            RuleContainerLevelCalculator = ruleContainerLevelCalculator;
            RuleStorageHierarchyCalculator = ruleStorageHierarchyCalculator;
            StorageOperationsManager = storageOperationsManager;
            DistributionManager = distributionManager;
        }

        public IMaterialManager MaterialManager { get; private set; }

        public IUserManager UserManager { get; private set; }

        public IContainerManager ContainerManager { get; private set; }

        public IHistoryManager HistoryManager { get; private set; }

        public IRuleContainerLevelCalculator RuleContainerLevelCalculator { get; private set; }

        public IRuleStorageHierarchyCalculator RuleStorageHierarchyCalculator { get; private set; }

        public IStorageOperationsManager StorageOperationsManager { get; private set; }

        public IDistributionManager DistributionManager { get; private set; }
    }
}