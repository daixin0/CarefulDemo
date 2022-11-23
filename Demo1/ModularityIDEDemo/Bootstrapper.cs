using Careful.BootstrapperApplication;
using Careful.Controls.MessageBoxControl;
using Careful.Core.DialogServices;
using Careful.Core.Ioc;
using Careful.Core.Mvvm.BindingExtension;
using Careful.Module.Core.Modularity;
using Microsoft.Practices.ServiceLocation;
using Module.Test.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ModularityIDEDemo
{
    public class Bootstrapper : CarefulBootstrapper
    {
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            foreach (var item in files)
            {
                var file = string.Format("{0}.dll", System.IO.Path.Combine(Path.GetDirectoryName(item), assemblyName.Name));

                if (File.Exists(file))
                {
                    return Assembly.LoadFile(file);
                }
            }

            return args.RequestingAssembly;
        }
        protected override DependencyObject CreateShell()
        {
            return new MainWindow() ;
        }
        protected override void InitIoc()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            Application application = null;
            if (Application.Current != null)
            {
                application = Application.Current;
            }
            else
            {
                application = new Application();
            }
            application.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //if (!CarefulIoc.Default.IsRegistered<Application>())
            //{
            //    application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //    application.Exit += Application_Exit;

            //}
            //ResourceDictionary controlTheme = (ResourceDictionary)Application.LoadComponent(
            //                new Uri("/Careful.Controls;component/Theme.xaml", UriKind.Relative));

            //Application.Current.Resources.MergedDictionaries.Add(controlTheme);
            CarefulIoc.Default.RegisterInstance<Application>(application);
            CarefulIoc.Default.Register<IMessageView, MessageBoxWindow>(false);


        }
        public override void Run(bool runWithDefaultConfiguration)
        {
            //XamlReader序列化时保留Binding
            EditorHelper.Register<BindingExpression, BindingConvertor>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(typeof(DesignerModule));
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
