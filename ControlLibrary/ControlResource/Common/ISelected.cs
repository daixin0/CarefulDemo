using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.Common
{
    public interface ISelected: INotifyPropertyChanged
    {
        bool IsSelected { get; set; }
    }
    public class Selected : NotifyPropertyChanged, ISelected
    {
        private bool _isSelected;

        /// <summary>
        /// Get or set IsSelected value
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }
    }
    
}
