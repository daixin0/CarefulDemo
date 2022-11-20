using Careful.Core.Mvvm.Command;
using Careful.Core.Mvvm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Module.Test.ViewModels
{
    public class MenuToolViewModel: ViewModelBase
    {
        
        public ICommand NewCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    //IInputElement inputElement = new IInputElement();
                    //ApplicationCommands.New.Execute(null, inputElement);
                });
            }
        }

    }
}
