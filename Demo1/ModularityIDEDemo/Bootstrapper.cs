using Careful.BootstrapperApplication;
using Microsoft.Practices.ServiceLocation;
using Module.Test.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModularityIDEDemo
{
    public class Bootstrapper : CarefulBootstrapper
    {
        //protected override DependencyObject CreateShell()
        //{
        //    return ServiceLocator.Current.GetInstance<MainWindowView>();
        //}
        public override void Run(bool runWithDefaultConfiguration)
        {
            MainWindowView mainWindowView = new MainWindowView();
            mainWindowView.ShowDialog();
        }
    }
}
