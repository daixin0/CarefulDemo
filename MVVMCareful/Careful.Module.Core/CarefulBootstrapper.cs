using Careful.Core.Extensions;
using Careful.Core.Ioc;
using Careful.Core.Logs;
using Careful.Core.MessageFrame.Events;
using Careful.Core.Mvvm;
using Careful.Module.Core.Modularity;
using Careful.Module.Core.Regions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Module.Core
{
    public class CarefulBootstrapper : Bootstrapper
    {
        private bool useDefaultConfiguration = true;

        /// <summary>
        /// Gets the default <see cref="IUnityContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IUnityContainer"/> instance.</value>
        [CLSCompliant(false)]
        public ICarefulIoc Container { get; protected set; }


        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default Prism Library services in the container. This is the default behavior.</param>
        public override void Run(bool runWithDefaultConfiguration)
        {
            this.useDefaultConfiguration = runWithDefaultConfiguration;

            this.Logger = this.CreateLogger();
            if (this.Logger == null)
            {
                throw new InvalidOperationException("log created faild");
            }

            this.Logger.Log("log created success", LogLevel.Debug, Priority.Low);

            this.Logger.Log("create module catelog", LogLevel.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException("module catelog null");
            }

            this.Logger.Log("configuring module catelog", LogLevel.Debug, Priority.Low);
            this.ConfigureModuleCatalog();

            this.Logger.Log("creating careful container", LogLevel.Debug, Priority.Low);
            this.Container = this.CreateContainer();
            if (this.Container == null)
            {
                throw new InvalidOperationException("careful container null");
            }

            this.Logger.Log("configuring careful container", LogLevel.Debug, Priority.Low);
            this.ConfigureContainer();

            this.Logger.Log("confguring service locator", LogLevel.Debug, Priority.Low);
            this.ConfigureServiceLocator();

            this.Logger.Log("configuring region adapter", LogLevel.Debug, Priority.Low);
            this.ConfigureRegionAdapterMappings();

            this.Logger.Log("configuring default region behaviors", LogLevel.Debug, Priority.Low);
            this.ConfigureDefaultRegionBehaviors();

            this.Logger.Log("registering frameworkd excetion type", LogLevel.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log("create shell", LogLevel.Debug, Priority.Low);
            this.Shell = this.CreateShell();
            if (this.Shell != null)
            {
                this.Logger.Log("setting region management", LogLevel.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, this.Container.GetInstance<IRegionManager>());

                this.Logger.Log("updating region", LogLevel.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                this.Logger.Log("initialize shell", LogLevel.Debug, Priority.Low);
                this.InitializeShell();
            }

            if (this.Container.IsRegistered<IModuleManager>())
            {
                this.Logger.Log("initialize module", LogLevel.Debug, Priority.Low);
                this.InitializeModules();
            }

            this.Logger.Log("bootstrapper sequence completed", LogLevel.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => this.Container.GetInstance<IServiceLocator>());
        }

        /// <summary>
        /// Registers in the <see cref="IUnityContainer"/> the <see cref="Type"/> of the Exceptions
        /// that are not considered root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.Register<ILog>();


            this.Logger.Log("adding careful bootstrapper extension to container", LogLevel.Debug, Priority.Low);
            

            this.Container.RegisterInstance<IModuleCatalog>(this.ModuleCatalog);

            if (useDefaultConfiguration)
            {
                //RegisterTypeIfMissing(typeof(IServiceLocator), typeof(ServiceLocator), true);
                RegisterTypeIfMissing(typeof(IModuleInitializer), typeof(ModuleInitializer), true);
                RegisterTypeIfMissing(typeof(IModuleManager), typeof(ModuleManager), true);
                RegisterTypeIfMissing(typeof(RegionAdapterMappings), typeof(RegionAdapterMappings), true);
                RegisterTypeIfMissing(typeof(IRegionManager), typeof(RegionManager), true);
                RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
                RegisterTypeIfMissing(typeof(IRegionViewRegistry), typeof(RegionViewRegistry), true);
                RegisterTypeIfMissing(typeof(IRegionBehaviorFactory), typeof(RegionBehaviorFactory), true);
                RegisterTypeIfMissing(typeof(IRegionNavigationJournalEntry), typeof(RegionNavigationJournalEntry), false);
                RegisterTypeIfMissing(typeof(IRegionNavigationJournal), typeof(RegionNavigationJournal), false);
                RegisterTypeIfMissing(typeof(IRegionNavigationService), typeof(RegionNavigationService), false);
                RegisterTypeIfMissing(typeof(IRegionNavigationContentLoader), typeof(RegionNavigationContentLoader), true);
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = this.Container.GetInstance<IModuleManager>();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException("null module catelog exception");
                }

                throw;
            }

            manager.Run();
        }

        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/>.</returns>
        [CLSCompliant(false)]
        protected virtual ICarefulIoc CreateContainer()
        {
            return new CarefulIoc();
        }

        /// <summary>
        /// Registers a type in the container only if that type was not already registered.
        /// </summary>
        /// <param name="fromType">The interface type to register.</param>
        /// <param name="toType">The type implementing the interface.</param>
        /// <param name="registerAsSingleton">Registers the type as a singleton.</param>
        protected void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
            }
            if (toType == null)
            {
                throw new ArgumentNullException("toType");
            }
            if (Container.IsRegistered(fromType))
            {
                Logger.Log(
                    String.Format(CultureInfo.CurrentCulture,
                                  "type mapping already registering",
                                  fromType.Name), LogLevel.Debug, Priority.Low);
            }
            else
            {
                if (registerAsSingleton)
                {
                    //Container.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
                }
                else
                {
                    //Container.RegisterType(fromType, toType);
                }
            }
        }

        protected override DependencyObject CreateShell()
        {
            throw new NotImplementedException();
        }
    }
}
