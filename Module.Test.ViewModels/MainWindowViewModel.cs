
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
    public class MainWindowViewModel:ViewModelDialog
    {

		public MainWindowViewModel(IEventAggregator eventAggregator)
		{

		}

		public ICommand LoadedCommand
		{
			get
			{
				return new RelayCommand((p) =>
				{

				});
			}
		}



	}
}
