using CommonEF;
using ContainerDAL;
using ContainerLevels;
using ContainerService;
using Distribution.Services;
using DistributionDAL;
using DistributionService;
using HistoryDAL;
using HistoryService;
using Infrastructure.Container.Services;
using Infrastructure.Distribution.Services;
using Infrastructure.History.Services;
using Infrastructure.Material.Services;
using Infrastructure.Users.Services;
using MaterialDAL;
using MaterialService;
using StorageHierarchy;
using StorageOperations;
using UserService;
using UsersDAL;

using System;

using Unity;
using Unity.Injection;
using CommonEF.Services;

namespace CryogattServerAPI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterSingleton<Cryogatt>(new InjectionConstructor());
            container.RegisterType<IContextFactory, ContextFactory>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IContainerRepository, ContainerRepository>();
            container.RegisterType<IHistoryRepository, HistoryRepository>();
            container.RegisterType<IMaterialRepository, MaterialRepository>();
            container.RegisterType<IDistributionRepository, DistributionRepository>();

            container.RegisterType<IUserManager, UserManager>();
            container.RegisterType<IContainerManager, ContainerManager>();
            container.RegisterType<IHistoryManager, HistoryManager>();
            container.RegisterType<IMaterialManager, MaterialManager>();
            container.RegisterType<IDistributionManager, DistributionManager>();

            container.RegisterType<IRuleContainerLevelCalculator, RuleContainerLevelCalculator>();
            container.RegisterType<IRuleStorageHierarchyCalculator, RuleStorageHierarchyCalculator>();
            container.RegisterType<IStorageOperationsManager, StorageOperationsManager>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();
        }
    }
}