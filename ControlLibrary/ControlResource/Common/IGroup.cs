using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Careful.Controls.Common
{
    public interface IGroup
    {
        bool? IsSelected { get; set; }
        ObservableCollection<IGroupItem> Children { get; set; }
        ICommand GroupCheckCommand { get; set; }
    }
    public class Group : NotifyPropertyChanged, IGroup
    {
        private bool? _isSelected;

        /// <summary>
        /// Get or set IsSelected value
        /// </summary>
        public bool? IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        private ICommand _groupCheckCommand;

        /// <summary>
        /// Get or set GroupCheckCommand value
        /// </summary>
        public ICommand GroupCheckCommand
        {
            get { return _groupCheckCommand; }
            set { Set(ref _groupCheckCommand, value); }
        }


        private ObservableCollection<IGroupItem> _children;


        /// <summary>
        /// Get or set Children value
        /// </summary>
        public ObservableCollection<IGroupItem> Children
        {
            get { return _children; }
            set { Set(ref _children, value); }
        }
    }
    public interface IGroupItem : ISelected
    {
        IGroup ParentGroup { get; set; }
    }
    public class GroupItem : NotifyPropertyChanged, IGroupItem
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
        private IGroup _parentGroup;

        /// <summary>
        /// Get or set ParentGroup value
        /// </summary>
        public IGroup ParentGroup
        {
            get { return _parentGroup; }
            set { Set(ref _parentGroup, value); }
        }

    }
}
