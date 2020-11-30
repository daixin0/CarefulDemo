using Careful.BootstrapperApplication;
using Careful.Controls.MessageBoxControl;
using Careful.Core.DialogServices;
using Careful.Core.Ioc;
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
        public override void Run(bool runWithDefaultConfiguration)
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            //Application application = null;
            //if (!CarefulIoc.Default.IsRegistered<Application>())
            //{
            //    application = new Application();
            //    application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //    application.Exit += Application_Exit;
                
            //}
            //ResourceDictionary controlTheme = (ResourceDictionary)Application.LoadComponent(
            //                new Uri("/Careful.Controls;component/Theme.xaml", UriKind.Relative));

            //Application.Current.Resources.MergedDictionaries.Add(controlTheme);
            //CarefulIoc.Default.RegisterInstance<Application>(application);
            CarefulIoc.Default.Register<IMessageView, MessageBoxWindow>(false);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
