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
    public class TreeView: NotifyPropertyChanged
    {
        private string _treeName;

        /// <summary>
        /// Get or set TreeName value
        /// </summary>
        public string TreeName
        {
            get { return _treeName; }
            set { Set(ref _treeName, value); }
        }
        private ObservableCollection<TreeView> _children;

        /// <summary>
        /// Get or set Children value
        /// </summary>
        public ObservableCollection<TreeView> Children
        {
            get { return _children; }
            set { Set(ref _children, value); }
        }

    }

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
