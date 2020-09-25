using Careful.Controls.DataGridControl.ConvertStringToLambda;
using Careful.Core.Mvvm.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Careful.Controls.DataGridControl.Enums;

namespace Careful.Controls.DataGridControl
{
    public class FilterDataViewModel: INotifyPropertyChanged
    {

        private static FilterDataViewModel _instance;
        
        public static FilterDataViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FilterDataViewModel();
                    
                }
                return _instance;
            }
            set { _instance = value; }
        }

        public event EventHandler ClearFilterEvent;
        public event EventHandler StartFilterEvent;

        #region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion
        public DataGridExtend Grid { get; set; }

        private ObservableCollection<FilterModel> _gridFilterColumn;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public ObservableCollection<FilterModel> GridFilterColumn
        {
            get { return _gridFilterColumn; }
            set
            {
                _gridFilterColumn = value;
                OnPropertyChanged("GridFilterColumn");
            }
        }
        private FilterModel _gridFilterColumnSelected;

        /// <summary>
        /// Get or set LeftBrackets value
        /// </summary>
        public FilterModel GridFilterColumnSelected
        {
            get { return _gridFilterColumnSelected; }
            set
            {
                _gridFilterColumnSelected = value;
                OnPropertyChanged("GridFilterColumnSelected");
            }
        }

        public ICommand WindowLoadCmd
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    Grid.RegisterOptionWindow();
                    GridFilterColumn = new ObservableCollection<FilterModel>();
                    GridFilterColumn.Add(new FilterModel() { Grid = this.Grid, ColumnValue = "", IsNoFirstItem = false });
                });
            }
        }
        public ICommand AddCmd
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    GridFilterColumn.Add(new FilterModel() { Grid = this.Grid, ColumnValue = "" });
                });
            }
        }
        public ICommand ClearCmd
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    GridFilterColumn.Clear();

                    GridFilterColumn.Add(new FilterModel() { Grid = this.Grid, ColumnValue = "", IsNoFirstItem = false });

                    if (ClearFilterEvent != null)
                    {
                        ClearFilterEvent(null, null);
                    }
                });
            }
        }

        public ICommand DeleteCmd
        {
            get
            {
                return new RelayCommand<FilterModel>((p) =>
                {
                    GridFilterColumn.Remove(p);
                });
            }
        }
        public Predicate<object> FilterPredicate = null;
        public ICommand ConfirmCmd
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    string code = "";
                    int count = 0;
                    if (GridFilterColumn == null)
                        return;
                    object[] valueList = new object[GridFilterColumn.Count];
                    foreach (var item in GridFilterColumn)
                    {
                        if (item.ColumnsNameSelected.ColumnField == null || item.FilterOperationsSelected == null)
                            continue;
                        if (!string.IsNullOrWhiteSpace(item.LeftBracketsSelected))
                        {
                            code += item.LeftBracketsSelected;
                        }
                        if (item.ColumnsNameSelected != null)
                        {
                            //code += "(p as Customer).";
                            if(item.ColumnsNameSelected.ColumnsType.Equals(typeof(string)) || item.ColumnsNameSelected.ColumnsType.Equals(typeof(object)))
                            {
                                code += item.ColumnsNameSelected.ColumnField + "!=null&&" + item.ColumnsNameSelected.ColumnField;
                            }
                            else
                            {
                                code += item.ColumnsNameSelected.ColumnField;
                            }
                            
                        }
                        if (item.FilterOperationsSelected != null)
                        {
                            if (item.FilterOperationsSelected.FilterOption == FilterOperation.Contains)
                            {
                                code += ".Contains(@" + count + ")";
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.StartsWith)
                            {
                                code += ".StartsWith(@" + count + ")";
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.EndsWith)
                            {
                                code += ".EndsWith(@" + count + ")";
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.Equals)
                            {
                                code += ".Equals(@" + count + ")";
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.GreaterThan)
                            {
                                code += ">@" + count;
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.GreaterThanEqual)
                            {
                                code += ">=@" + count;
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.LessThan)
                            {
                                code += "<@" + count;
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.LessThanEqual)
                            {
                                code += "<=@" + count;
                            }
                            else if (item.FilterOperationsSelected.FilterOption == FilterOperation.NotEquals)
                            {
                                code += "!=@" + count;
                            }
                            valueList[count] = TypeHelper.ValueConvertor(item.ColumnsNameSelected.ColumnsType, item.ColumnValue);
                        }
                        if (!string.IsNullOrWhiteSpace(item.RightBracketsSelected))
                        {
                            code += item.RightBracketsSelected;
                        }
                        if ((count + 1) != GridFilterColumn.Count)
                        {
                            if (item.LogicBracketsSelected == "And")
                            {
                                code += "&&";
                            }
                            else if (item.LogicBracketsSelected == "Or")
                            {
                                code += "||";
                            }
                        }
                        count++;
                    }
                    if (string.IsNullOrWhiteSpace(code) || valueList.Length <= 0)
                        return;
                    
                    IQueryable qq = Grid.ItemsSource.AsQueryable() as IQueryable;

                    FilterPredicate = ConvertTo.ConvertStringToLambda(Grid.FilterType, qq.Expression, new ParsingConfig(), code, valueList);

                    OnPropertyChanged("FilterChanged");

                    if (StartFilterEvent != null)
                        StartFilterEvent(null, null);
                });
            }
        }
    }
}
