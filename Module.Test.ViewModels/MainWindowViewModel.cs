
using Careful.Core.MessageFrame.Events;
using Careful.Core.Mvvm.Command;
using Careful.Core.Mvvm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Module.Test.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        private TreeView _treeViewModel;

        /// <summary>
        /// Get or set TreeViewModel value
        /// </summary>
        public TreeView TreeViewModel
        {
            get { return _treeViewModel; }
            set { Set(ref _treeViewModel, value); }
        }


        public MainWindowViewModel(IEventAggregator eventAggregator)
		{

		}

		public ICommand LoadedCommand
		{
			get
			{
				return new RelayCommand((p) =>
				{
                    TreeViewModel = new TreeView();
                    TreeViewModel.Children = new System.Collections.ObjectModel.ObservableCollection<TreeView>();
                    TreeViewModel.Children.Add(new TreeView() { TreeName="level1"});

                });
			}
		}



        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand<TreeView>((p) =>
                {
                    if (p.Children == null)
                        p.Children = new System.Collections.ObjectModel.ObservableCollection<TreeView>();
                    p.Children.Add(new TreeView() { TreeName="添加的节点"});
                });
            }
        }




    }
}
