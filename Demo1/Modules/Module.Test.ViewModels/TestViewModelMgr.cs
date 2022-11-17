using Careful.Core.Ioc;
using Careful.Core.Mvvm.PropertyChanged;
using Careful.Module.Core.Modularity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Test.ViewModels
{
    public class TestViewModelMgr : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<MainWindowViewModel>();
        }
    }
}
