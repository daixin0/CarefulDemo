using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Careful.Controls.DataGridControl
{
    public class FilterModel: INotifyPropertyChanged
    {

        public FilterModel()
        {
            LeftBrackets = new ObservableCollection<string>();
            LeftBrackets.Add("");
            LeftBrackets.Add("(");
            LeftBrackets.Add("((");
            LeftBrackets.Add("(((");
            RightBrackets = new ObservableCollection<string>();
            RightBrackets.Add("");
            RightBrackets.Add(")");
            RightBrackets.Add("))");
            RightBrackets.Add(")))");

            LogicBrackets = new ObservableCollection<string>();
            LogicBrackets.Add("");
            LogicBrackets.Add("And");
            LogicBrackets.Add("Or");
            GridColumnsName = new ObservableCollection<ColumnInfo>();
            ColumnsNameSelected = new ColumnInfo();
            FilterOperations = new ObservableCollection<FilterOperationItem>();

            BoolPropertyValues = new ObservableCollection<CheckboxComboItem>();
        }

        #region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        private ObservableCollection<string> _leftBrackets;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public ObservableCollection<string> LeftBrackets
        {
            get { return _leftBrackets; }
            set
            {
                _leftBrackets = value;
                OnPropertyChanged("LeftBrackets");
            }
        }
        private ObservableCollection<string> _rightBrackets;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public ObservableCollection<string> RightBrackets
        {
            get { return _rightBrackets; }
            set
            {
                _rightBrackets = value;
                OnPropertyChanged("RightBrackets");
            }
        }
        private ObservableCollection<string> _logicBrackets;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public ObservableCollection<string> LogicBrackets
        {
            get { return _logicBrackets; }
            set
            {
                _logicBrackets = value;
                OnPropertyChanged("LogicBrackets");
            }
        }
        private ObservableCollection<ColumnInfo> _gridColumnsName;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public ObservableCollection<ColumnInfo> GridColumnsName
        {
            get { return _gridColumnsName; }
            set
            {
                _gridColumnsName = value;
                OnPropertyChanged("GridColumnsName");
            }
        }
        private ObservableCollection<FilterOperationItem> _filterOperations;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public ObservableCollection<FilterOperationItem> FilterOperations
        {
            get { return _filterOperations; }
            set
            {
                _filterOperations = value;
                OnPropertyChanged("FilterOperations");
            }
        }
        private FilterOperationItem _filterOperationsSelected;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public FilterOperationItem FilterOperationsSelected
        {
            get { return _filterOperationsSelected; }
            set
            {
                _filterOperationsSelected = value;
                OnPropertyChanged("FilterOperationsSelected");
            }
        }

        private bool _isNoFirstItem = true;

        /// <summary>
        /// Get or set IsNoFirstItem value
        /// </summary>
        public bool IsNoFirstItem
        {
            get { return _isNoFirstItem; }
            set {
                _isNoFirstItem = value;
                OnPropertyChanged("IsNoFirstItem");
            }
        }

        public ObservableCollection<CheckboxComboItem> BoolPropertyValues { get; set; }

        public CheckboxComboItem BoolPropertyValueSelected { get; set; }

        public string LeftBracketsSelected { get; set; }
        

        public string RightBracketsSelected { get; set; }

        public ColumnInfo ColumnsNameSelected { get; set; }
        public string LogicBracketsSelected { get; set; }

        public string ColumnValue { get; set; }

        public DataGridExtend Grid { get; set; }
    }
    public class ColumnInfo
    {
        public string ColumnTitle { get; set; }

        public string ColumnField { get; set; }

        public Type ColumnsType { get; set; }

        public DataGridColumn DataGridColumn { get; set; }

        public DataGridColumnHeader DataGridColumnHeader { get; set; }
    }
}
