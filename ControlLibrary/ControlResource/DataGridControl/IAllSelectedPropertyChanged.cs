using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.DataGridControl
{
    public interface IAllSelectedPropertyChanged
    {
        string IsSelected { get; set; }
    }
    public class AllSelectedPropertyChanged : NotifyPropertyChanged, IAllSelectedPropertyChanged
    {

        private int _num;

        /// <summary>
        /// Get or set IsSelected value
        /// </summary>
        public int Num
        {
            get { return _num; }
            set {
                Set(ref _num, value);
            }
        }

        private string _isSelected = "";

        /// <summary>
        /// Get or set IsSelected value
        /// </summary>
        public string IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }


    }
}
