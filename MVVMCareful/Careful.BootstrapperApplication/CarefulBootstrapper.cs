using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Careful.Controls.MessageBoxControl;
using Careful.Core.DialogServices;
using Careful.Core.Extensions;
using Careful.Core.Ioc;
using Careful.Core.Logs;
using Careful.Core.MessageFrame.Events;
using Careful.Core.Mvvm.ViewModel;
using Careful.Module.Core.Modularity;
using Careful.Module.Core.Regions;
using Careful.Module.Core.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;

namespace Careful.BootstrapperApplication
{
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence and hooks
    /// that specific implementations can override
    /// </summary>
    /// <remarks>
    /// This class must be overridden to provide application specific configuration.
    /// </remarks>
    public abstract class CarefulBootstrapper
    {
        /// <summary>
        /// Gets the <see cref="ILog"/> for the application.
        /// </summary>
        /// <value>A <see cref="ILog"/> instance.</value>
        protected ILog Logger { get; set; }

        /// <summary>
        /// Gets the default <see cref="IModuleCatalog"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IModuleCatalog"/> instance.</value>
        protected IModuleCatalog ModuleCatalog { get; set; }
        public IContainerProvider Provider => ContainerExtension;
        public IContainerExtension ContainerExtension { get; set; }

        public ICarefulIoc Container { get; protected set; }

        /// <summary>
        /// Gets the shell user interface
        /// </summary>
        /// <value>The shell user interface.</value>
        protected DependencyObject Shell { get; set; }

        /// <summary>
        /// Create the <see cref="ILog" /> used by the bootstrapper.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new TextLogger.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "The Logger is added to the container which will dispose it when the container goes out of scope.")]
        protected virtual ILog CreateLogger()
        {
            return new FileLogger();
        }

        /// <summary>
        /// Runs the bootstrapper process.
        /// </summary>
        public void Run()
        {
            Initialize();
            Run(true);
        }
        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/>.</returns>
        protected virtual ICarefulIoc CreateContainer()
        {
            return CarefulIoc.Default;
        }

        protected virtual IContainerExtension CreateContainerExtension()
        {
            return CarefulIoc.Default.GetInstance<IContainerExtension>();
        }
        #region configure
        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.RegisterInstance<ILog>(Logger);

            this.Logger.Log("adding careful bootstrapper extension to container", LogLevel.Debug, Priority.Low);
            //Container.RegisterInstance<ICarefulIoc>(Container);
            this.Container.RegisterInstance<IModuleCatalog>(this.ModuleCatalog);
            Container.RegisterInstance<ILog>(Logger);
            Container.Register<IDialogService, DialogService>();

            Container.RegisterInstance<IServiceLocator>(CarefulIoc.Default);
            Container.Register<IModuleInitializer, ModuleInitializer>();
            Container.Register<IModuleManager, ModuleManager>();
            Container.Register<IEventAggregator, EventAggregator>();
            Container.Register<IContainerExtension, CarefulIocExtension>();
            //Container.Register<RegionAdapterMappings>(true);
            //Container.Register<IRegionManager, RegionManager>(true);
            //Container.Register<IRegionViewRegistry, RegionViewRegistry>(true);
            //Container.Register<IRegionBehaviorFactory, RegionBehaviorFactory>(true);
            //Container.Register<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>(false);
            //Container.Register<IRegionNavigationJournal, RegionNavigationJournal>(false);
            //Container.Register<IRegionNavigationContentLoader, RegionNavigationContentLoader>(false);
            //Container.Register<IRegionNavigationService, RegionNavigationService>(false);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        protected virtual void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => CarefulIoc.Default);
        }

        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) { }
        protected virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type, isSingle) =>
            {
                if (isSingle)
                {
                    return ContainerLocator.Container.Resolve(type);
                }
                else
                {
                    return ContainerLocator.Container.Resolve(type, Guid.NewGuid().ToString());
                }

            });
        }
        #endregion

        /// <summary>
        /// Creates the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        ///  <remarks>
        /// The base implementation returns a new ModuleCatalog.
        /// </remarks>
        protected virtual IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) };
        }
        protected virtual void InitIoc()
        {

        }
        protected virtual void Initialize()
        {

            InitIoc();
            this.Logger = this.CreateLogger();
            if (this.Logger == null)
            {
                throw new InvalidOperationException("log created faild");
            }


            ConfigureViewModelLocator();
            this.Logger.Log("log created success", LogLevel.Debug, Priority.Low);

            this.Logger.Log("create module catelog", LogLevel.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException("module catelog null");
            }

            this.Logger.Log("configuring module catelog", LogLevel.Debug, Priority.Low);
            ConfigureModuleCatalog(ModuleCatalog);

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

            ContainerLocator.SetContainerExtension(() => CreateContainerExtension());

            ContainerExtension = ContainerLocator.Current;


            //RegisterRequiredTypes(ContainerExtension);
            //RegisterTypes(ContainerExtension);
            //ContainerExtension.FinalizeExtension();

            this.Logger.Log("configuring region adapter", LogLevel.Debug, Priority.Low);
            //this.ConfigureRegionAdapterMappings();


            this.Logger.Log("registering frameworkd excetion type", LogLevel.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log("create shell", LogLevel.Debug, Priority.Low);

            this.Shell = this.CreateShell();
            if (this.Shell != null)
            {
                MvvmHelpers.AutowireViewModel(Shell);

                this.Logger.Log("initialize shell", LogLevel.Debug, Priority.Low);
                this.OnInitialized();
            }

            if (this.Container.IsRegistered<IModuleManager>())
            {
                this.Logger.Log("initialize module", LogLevel.Debug, Priority.Low);
                this.InitializeModules();
            }

            this.Logger.Log("bootstrapper sequence completed", LogLevel.Debug, Priority.Low);



            //var regionAdapterMappins = ContainerExtension.Resolve<RegionAdapterMappings>();
            //ConfigureRegionAdapterMappings(regionAdapterMappins);

            //var defaultRegionBehaviors = ContainerExtension.Resolve<IRegionBehaviorFactory>();
            //ConfigureDefaultRegionBehaviors(defaultRegionBehaviors);

            //RegisterFrameworkExceptionTypes();
        }


        /// <summary>
        /// Registers all types that are required by Prism to function with the container.
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected virtual void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            if (ModuleCatalog == null)
                throw new InvalidOperationException("IModuleCatalog");

            containerRegistry.RegisterInstance(ModuleCatalog);
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<IModuleInitializer, ModuleInitializer>();
            containerRegistry.RegisterSingleton<IModuleManager, ModuleManager>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            //containerRegistry.Register<IDialogWindow, DialogWindow>(); //default dialog host
        }
        protected virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        /// <summary>
        /// Registers the <see cref="Type"/>s of the Exceptions that are not considered 
        /// root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected virtual void RegisterFrameworkExceptionTypes()
        {
            ExceptionExtensions.RegisterFrameworkExceptionType(
                typeof(Microsoft.Practices.ServiceLocation.ActivationException));
        }


        /// <summary>
        /// Contains actions that should occur last.
        /// </summary>
        protected virtual void OnInitialized()
        {
            if (Shell is Window window)
                window.Show();
        }
        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default 
        /// Prism Library services in the container. This is the default behavior.</param>
        public abstract void Run(bool runWithDefaultConfiguration);

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        /// If the returned instance is a <see cref="DependencyObject"/>, the
        /// <see cref="CarefulBootstrapper"/> will attach the default <see cref="IRegionManager"/> of
        /// the application in its <see cref="RegionManager.RegionManagerProperty"/> attached property
        /// in order to be able to add regions by using the <see cref="RegionManager.RegionNameProperty"/>
        /// attached property from XAML.
        /// </remarks>
        protected virtual DependencyObject CreateShell()
        {
            return null;
        }


        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected virtual void InitializeModules()
        {
            IModuleManager manager = ServiceLocator.Current.GetInstance<IModuleManager>();
            manager.Run();
        }
    }
}
