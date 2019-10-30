using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Controls.Primitives;
using static ControlResource.ExtendControlStyle.DataGrid.Enums;
using ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda;

namespace ControlResource.ExtendControlStyle.DataGrid
{
    /// <summary>
    /// Interaction logic for ColumnFilterControl.xaml
    /// </summary>
    public partial class ColumnFilterControl : UserControl, INotifyPropertyChanged
    {
        private Func<object, object> _boundColumnPropertyAccessor = null;

        #region Properties

        public ObservableCollection<FilterOperationItem> FilterOperations { get; set; }

        public ObservableCollection<CheckboxComboItem> DistinctPropertyValues { get; set; }

        public bool HasPredicate
        {
            get
            {
                if (FilterText == null)
                    return false;
                return FilterText.Length > 0 || DistinctPropertyValues.Where(d => d.IsChecked).Count() > 0;
            }
        }

        public OptionColumnInfo FilterColumnInfo { get; set; }

        public DataGridExtend Grid { get; set; }

        private bool _CanUserFreeze = true;
        public bool CanUserFreeze
        {
            get
            {
                return _CanUserFreeze;
            }
            set
            {
                _CanUserFreeze = value;
                Grid.UpdateColumnOptionControl(this);
                OnPropertyChanged("CanUserFreeze");
            }
        }

        private bool _CanUserGroup;
        public bool CanUserGroup
        {
            get
            {
                return _CanUserGroup;
            }
            set
            {
                _CanUserGroup = value;
                Grid.UpdateColumnOptionControl(this);
                OnPropertyChanged("CanUserGroup");
            }
        }

        private bool _CanUserSelectDistinct = false;
        public bool CanUserSelectDistinct
        {
            get
            {
                return _CanUserSelectDistinct;
            }
            set
            {
                _CanUserSelectDistinct = value;
            }
        }

        public Visibility FilterVisibility
        {
            get
            {
                return this.Visibility;
            }
            set
            {
                this.Visibility = value;
            }
        }

        public bool FilterOperationsEnabled
        {
            get { return DistinctPropertyValues.Where(i => i.IsChecked).Count() == 0; }
        }

        private string _FilterText = string.Empty;
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                if (value != _FilterText)
                {
                    _FilterText = value;
                    OnPropertyChanged("FilterText");
                    OnPropertyChanged("FilterChanged");
                }
            }
        }


        private FilterOperationItem _SelectedFilterOperation;
        public FilterOperationItem SelectedFilterOperation
        {
            get
            {
                if (DistinctPropertyValues.Where(i => i.IsChecked).Count() > 0)
                    return FilterOperations.Where(f => f.FilterOption == Enums.FilterOperation.Equals).FirstOrDefault();
                return _SelectedFilterOperation;
            }
            set
            {
                if (value != _SelectedFilterOperation)
                {
                    _SelectedFilterOperation = value;
                    OnPropertyChanged("SelectedFilterOperation");
                    OnPropertyChanged("FilterChanged");
                }
            }
        }
        #endregion

        public ColumnFilterControl()
        {
            DistinctPropertyValues = new ObservableCollection<CheckboxComboItem>();
            FilterOperations = new ObservableCollection<FilterOperationItem>();
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = this;
                this.Loaded += new RoutedEventHandler(ColumnFilterControl_Loaded);
            }
        }


        void ColumnFilterControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = null;
            DataGridColumnHeader colHeader = null;

            UIElement parent = (UIElement)VisualTreeHelper.GetParent(this);
            while (parent != null)
            {
                parent = (UIElement)VisualTreeHelper.GetParent(parent);
                if (colHeader == null)
                    colHeader = parent as DataGridColumnHeader;

                if (Grid == null)
                    Grid = parent as DataGridExtend;
            }

            if (colHeader != null)
                column = colHeader.Column;

            CanUserFreeze = Grid.CanUserFreeze;
            CanUserGroup = Grid.CanUserGroup;
            CanUserSelectDistinct = Grid.CanUserSelectDistinct;

            if (column != null)
            {
                object oCanUserFreeze = column.GetValue(ColumnConfiguration.CanUserFreezeProperty);
                if (oCanUserFreeze != null)
                    CanUserFreeze = (bool)oCanUserFreeze;

                object oCanUserGroup = column.GetValue(ColumnConfiguration.CanUserGroupProperty);
                if (oCanUserGroup != null)
                    CanUserGroup = (bool)oCanUserGroup;

                object oCanUserSelectDistinct = column.GetValue(ColumnConfiguration.CanUserSelectDistinctProperty);
                if (oCanUserSelectDistinct != null)
                    CanUserSelectDistinct = (bool)oCanUserSelectDistinct;
            }
            if (Grid.FilterType == null)
                return;

            FilterColumnInfo = new OptionColumnInfo(column, Grid);

            Grid.RegisterOptionControl(this);

            FilterOperations.Clear();
            if (FilterColumnInfo.PropertyType != null)
            {

                if (TypeHelper.IsNumbericType(FilterColumnInfo.PropertyType) && TypeHelper.IsStringType(FilterColumnInfo.PropertyConvertType))
                {
                    FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                    CalcControlVisibility(ColumnType.ConvertString);
                }
                else
                {
                    if (TypeHelper.IsStringType(FilterColumnInfo.PropertyType) || TypeHelper.IsStringType(FilterColumnInfo.PropertyConvertType))
                    {
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Contains, "Contains", "/Jib.WPF.Controls;component/Images/Contains.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.StartsWith, "Starts With", "/Jib.WPF.Controls;component/Images/StartsWith.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.EndsWith, "Ends With", "/Jib.WPF.Controls;component/Images/EndsWith.png"));
                        CalcControlVisibility(ColumnType.String);
                    }
                    FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                    if (TypeHelper.IsNumbericType(FilterColumnInfo.PropertyType))
                    {
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThan, ">", "/Jib.WPF.Controls;component/Images/GreaterThan.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThanEqual, ">=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThan, "<", "/Jib.WPF.Controls;component/Images/LessThan.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThanEqual, "<=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.NotEquals, "!=", "/Jib.WPF.Controls;component/Images/NotEqual.png"));
                        CalcControlVisibility(ColumnType.Num);
                    }
                    if (TypeHelper.IsBoolType(FilterColumnInfo.PropertyType) || TypeHelper.IsBoolType(FilterColumnInfo.PropertyConvertType))
                    {
                        CalcControlVisibility(ColumnType.Bool);
                    }
                    if (TypeHelper.IsDateTimeType(FilterColumnInfo.PropertyType))
                    {
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThan, ">", "/Jib.WPF.Controls;component/Images/GreaterThan.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThanEqual, ">=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThan, "<", "/Jib.WPF.Controls;component/Images/LessThan.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThanEqual, "<=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.NotEquals, "!=", "/Jib.WPF.Controls;component/Images/NotEqual.png"));
                        CalcControlVisibility(ColumnType.Date);
                    }
                }

                if (FilterOperations.Count > 0)
                    SelectedFilterOperation = FilterOperations[0];
            }

            if (FilterColumnInfo != null && FilterColumnInfo.IsValid)
            {
                foreach (var i in DistinctPropertyValues.Where(i => i.IsChecked))
                    i.IsChecked = false;
                DistinctPropertyValues.Clear();
                FilterText = string.Empty;
                _boundColumnPropertyAccessor = null;

                if (!string.IsNullOrWhiteSpace(FilterColumnInfo.PropertyPath))
                {
                    if (FilterColumnInfo.PropertyPath.Contains('.'))
                        throw new ArgumentException(string.Format("This version of the grid does not support a nested property path such as '{0}'.  Please make a first-level property for filtering and bind to that.", FilterColumnInfo.PropertyPath));

                    this.Visibility = System.Windows.Visibility.Visible;
                    ParameterExpression arg = System.Linq.Expressions.Expression.Parameter(typeof(object), "x");
                    System.Linq.Expressions.Expression expr = System.Linq.Expressions.Expression.Convert(arg, Grid.FilterType);
                    expr = System.Linq.Expressions.Expression.Property(expr, Grid.FilterType, FilterColumnInfo.PropertyPath);
                    System.Linq.Expressions.Expression conversion = System.Linq.Expressions.Expression.Convert(expr, typeof(object));
                    _boundColumnPropertyAccessor = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(conversion, arg).Compile();

                    if (_boundColumnPropertyAccessor != null)
                    {
                        if (DistinctPropertyValues.Count == 0)
                        {
                            //List<object> result = new List<object>();
                            //foreach (var i in Grid.ItemsSource)
                            //{
                            //    object value = _boundColumnPropertyAccessor(i);
                            //    if (value != null)
                            //    {
                            //        if (result.Where(o => o.ToString() == value.ToString()).Count() == 0)
                            //            result.Add(value);
                            //    }
                            //    else if (FilterColumnInfo.PropertyType == typeof(bool?) && FilterColumnInfo.IsSpecialState)
                            //    {
                            //        if (result.Where(o => o == value).Count() == 0)
                            //            result.Add(value);
                            //    }

                            //}
                            //try
                            //{
                            //    result.Sort();
                            //}
                            //catch
                            //{
                            //    if (System.Diagnostics.Debugger.IsLogging())
                            //        System.Diagnostics.Debugger.Log(0, "Warning", "There is no default compare set for the object type");
                            //}

                            //if (result.Count > 0)
                            //{
                            //    StringBuilder sb = new StringBuilder();
                            //    foreach (var obj in result)
                            //    {
                            //        var item = new CheckboxComboItem()
                            //        {
                            //            Description = GetFormattedValue(obj),
                            //            Tag = obj,
                            //            IsChecked = true
                            //        };
                            //        item.PropertyChanged += new PropertyChangedEventHandler(filter_PropertyChanged);
                            //        DistinctPropertyValues.Add(item);
                            //        sb.AppendFormat("{0}{1}", sb.Length > 0 ? "," : "",item.Description );
                            //    }

                            //    FilterText = sb.ToString();
                            //}
                            //else
                            //{
                            //    FilterText = string.Empty;
                            //}

                        }
                    }
                }
                else
                {
                    this.Visibility = System.Windows.Visibility.Collapsed;
                }
                object oDefaultFilter = column.GetValue(ColumnConfiguration.DefaultFilterProperty);
                if (oDefaultFilter != null)
                    FilterText = (string)oDefaultFilter;
            }

        }


        private void txtFilter_Loaded(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).DataContext = this;
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {

            if (FilterColumnInfo.Converter != null)
            {
                if (_boundColumnPropertyAccessor != null)
                {
                    List<object> result = new List<object>();
                    foreach (var i in Grid.FilteredItemsSource)
                    {
                        object value = _boundColumnPropertyAccessor(i);
                        if (value != null)
                        {
                            if (result.Where(o => o.ToString() == value.ToString()).Count() == 0)
                                result.Add(value);
                        }
                        else if (FilterColumnInfo.PropertyType == typeof(bool?) && FilterColumnInfo.IsSpecialState)
                        {
                            if (result.Where(o => o == value).Count() == 0)
                                result.Add(value);
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in result)
                    {
                        if (GetFormattedValue(item).Contains(((TextBox)sender).Text) && ((TextBox)sender).Text.Trim() != "")
                        {
                            //sb.Append(item);
                            sb.AppendFormat("{0}{1}", sb.Length > 0 ? "," : "", item == null ? "null" : item);
                        }
                    }
                    if (sb.ToString() == "")
                    {
                        FilterText = ((TextBox)sender).Text;
                    }
                    else
                    {
                        FilterText = sb.ToString();
                    }
                }
            }
            else
            {
                FilterText = ((TextBox)sender).Text;
            }

        }

        public Predicate<object> GeneratePredicate()
        {
            Predicate<object> predicate = null;
            if (DistinctPropertyValues.Where(i => i.IsChecked).Count() > 0)
            {
                foreach (var item in DistinctPropertyValues.Where(i => i.IsChecked))
                {
                    if (predicate == null)
                    {
                        if (FilterColumnInfo.PropertyType == typeof(bool?) && FilterColumnInfo.IsSpecialState)
                        {
                            predicate = GenerateFilterPredicate(FilterColumnInfo.PropertyPath, item.Tag == null ? "null" : item.Tag.ToString(), Grid.FilterType, FilterColumnInfo.PropertyType, SelectedFilterOperation);
                        }
                        else
                        {
                            predicate = GenerateFilterPredicate(FilterColumnInfo.PropertyPath, item.Tag.ToString(), Grid.FilterType, FilterColumnInfo.PropertyType, SelectedFilterOperation);
                        }
                    }

                    else
                        predicate = predicate.Or(GenerateFilterPredicate(FilterColumnInfo.PropertyPath, item.Tag.ToString(), Grid.FilterType, FilterColumnInfo.PropertyType.UnderlyingSystemType, SelectedFilterOperation));
                }
            }
            else
            {
                if (FilterColumnInfo.PropertyType == null)
                    return null;
                predicate = GenerateFilterPredicate(FilterColumnInfo.PropertyPath, FilterText, Grid.FilterType, FilterColumnInfo.PropertyType.UnderlyingSystemType, SelectedFilterOperation);
            }
            return predicate;
        }
        protected Predicate<object> GenerateFilterPredicate(string propertyName, string filterValue, Type objType, Type propType, FilterOperationItem filterItem)
        {
            ParameterExpression objParam = System.Linq.Expressions.Expression.Parameter(typeof(object), "x");
            UnaryExpression param = System.Linq.Expressions.Expression.TypeAs(objParam, objType);
            var prop = System.Linq.Expressions.Expression.Property(param, propertyName);
            var val = System.Linq.Expressions.Expression.Constant(filterValue);

            switch (filterItem.FilterOption)
            {
                case Enums.FilterOperation.Contains:
                    return ExpressionHelper.GenerateGeneric(prop, val, propType, objParam, "Contains");
                case Enums.FilterOperation.EndsWith:
                    return ExpressionHelper.GenerateGeneric(prop, val, propType, objParam, "EndsWith");
                case Enums.FilterOperation.StartsWith:
                    return ExpressionHelper.GenerateGeneric(prop, val, propType, objParam, "StartsWith");
                case Enums.FilterOperation.Equals:
                    DateTime dtEquals = new DateTime();
                    if (DateTime.TryParse(filterValue, out dtEquals))
                    {
                        return ExpressionHelper.GenerateEquals(prop, dtEquals, propType, objParam);
                    }
                    else
                    {
                        if (propertyName == "DebitCreditDirection" && (filterValue != "1" && filterValue != "-1" && filterValue != "0"))
                        {
                            return new Predicate<object>((p) => { return false; });
                        }
                        else
                        {
                            return ExpressionHelper.GenerateEquals(prop, filterValue, propType, objParam);
                        }

                    }
                case Enums.FilterOperation.NotEquals:
                    DateTime dtNotEquals = new DateTime();
                    if (DateTime.TryParse(filterValue, out dtNotEquals))
                    {
                        return ExpressionHelper.GenerateNotEquals(prop, dtNotEquals, propType, objParam);
                    }
                    else
                    {
                        return ExpressionHelper.GenerateNotEquals(prop, filterValue, propType, objParam);
                    }
                case Enums.FilterOperation.GreaterThanEqual:
                    DateTime dtGreaterThanEqual = new DateTime();
                    if (DateTime.TryParse(filterValue, out dtGreaterThanEqual))
                    {
                        return ExpressionHelper.GenerateGreaterThanEqual(prop, dtGreaterThanEqual, propType, objParam);
                    }
                    else
                    {
                        return ExpressionHelper.GenerateGreaterThanEqual(prop, filterValue, propType, objParam);
                    }
                case Enums.FilterOperation.LessThanEqual:
                    DateTime dtLessThanEqual = new DateTime();
                    if (DateTime.TryParse(filterValue, out dtLessThanEqual))
                    {
                        return ExpressionHelper.GenerateLessThanEqual(prop, dtLessThanEqual, propType, objParam);
                    }
                    else
                    {
                        return ExpressionHelper.GenerateLessThanEqual(prop, filterValue, propType, objParam);
                    }
                case Enums.FilterOperation.GreaterThan:
                    DateTime dtGreaterThan = new DateTime();
                    if (DateTime.TryParse(filterValue, out dtGreaterThan))
                    {
                        return ExpressionHelper.GenerateGreaterThan(prop, dtGreaterThan, propType, objParam);
                    }
                    else
                    {
                        return ExpressionHelper.GenerateGreaterThan(prop, filterValue, propType, objParam);
                    }
                case Enums.FilterOperation.LessThan:
                    DateTime dtLessThan = new DateTime();
                    if (DateTime.TryParse(filterValue, out dtLessThan))
                    {
                        return ExpressionHelper.GenerateLessThan(prop, dtLessThan, propType, objParam);
                    }
                    else
                    {
                        return ExpressionHelper.GenerateLessThan(prop, filterValue, propType, objParam);
                    }
                default:
                    throw new ArgumentException("Could not decode Search Mode.  Did you add a new value to the enum, or send in Unknown?");
            }

        }

        public void ResetControl()
        {
            for (int i = 0; i < DistinctPropertyValues.Count; i++)
            {
                DistinctPropertyValues[i].IsChecked = false;
            }
            //foreach (var i in DistinctPropertyValues)
            //    i.IsChecked = false;
            FilterText = string.Empty;
            this.txtFilter.Text = "";
            DistinctPropertyValues.Clear();
        }
        public void ResetDistinctList()
        {
            DistinctPropertyValues.Clear();
        }
        private void CalcControlVisibility(ColumnType type)
        {
            cbOperation.Visibility = System.Windows.Visibility.Collapsed;
            cbDistinctProperties.Visibility = System.Windows.Visibility.Collapsed;
            txtFilter.Visibility = System.Windows.Visibility.Collapsed;
            spDate.Visibility = Visibility.Collapsed;
            col1.Width = new GridLength(0);
            switch (type)
            {
                case ColumnType.String:
                    txtFilter.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ColumnType.Bool:
                    cbDistinctProperties.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ColumnType.Num:
                    col1.Width = new GridLength(25);
                    cbOperation.Visibility = System.Windows.Visibility.Visible;
                    txtFilter.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ColumnType.Date:
                    col1.Width = new GridLength(25);
                    cbOperation.Visibility = System.Windows.Visibility.Visible;
                    spDate.Visibility = Visibility.Visible;
                    break;
                case ColumnType.ConvertString:
                    //col1.Width = new GridLength(25);
                    //cbOperation.Visibility = System.Windows.Visibility.Visible;
                    txtFilter.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }



        private void cbDistinctProperties_DropDownOpened(object sender, EventArgs e)
        {
            if (_boundColumnPropertyAccessor != null)
            {
                DistinctPropertyValues.Clear();
                //if (DistinctPropertyValues.Count == 0)
                //{
                List<object> result = new List<object>();
                foreach (var i in Grid.ItemsSource)
                {
                    object value = _boundColumnPropertyAccessor(i);
                    if (value != null)
                    {
                        if (result.Where(o => o.ToString() == value.ToString()).Count() == 0)
                            result.Add(value);
                    }
                    else if (FilterColumnInfo.PropertyType == typeof(bool?) && FilterColumnInfo.IsSpecialState)
                    {
                        if (result.Where(o => o == value).Count() == 0)
                            result.Add(value);
                    }

                }
                try
                {
                    result.Sort();
                }
                catch
                {
                    if (System.Diagnostics.Debugger.IsLogging())
                        System.Diagnostics.Debugger.Log(0, "Warning", "There is no default compare set for the object type");
                }

                foreach (var obj in result)
                {
                    var item = new CheckboxComboItem()
                    {
                        Description = GetFormattedValue(obj),
                        Tag = obj,
                        IsChecked = true
                    };
                    item.PropertyChanged += new PropertyChangedEventHandler(filter_PropertyChanged);
                    DistinctPropertyValues.Add(item);

                }
                //}
            }
        }

        private string GetFormattedValue(object obj)
        {
            if (FilterColumnInfo.Converter != null && TypeHelper.IsBoolType(FilterColumnInfo.PropertyConvertType) && TypeHelper.IsStringType(FilterColumnInfo.PropertyType))
            {
                object result = FilterColumnInfo.Converter.Convert(obj, typeof(string), FilterColumnInfo.ConverterParameter, FilterColumnInfo.ConverterCultureInfo).ToString();
                if (result.ToString() == "True")
                {
                    return "已选";
                }
                else
                {
                    return "未选";
                }
            }
            else if (FilterColumnInfo.Converter != null)
                return FilterColumnInfo.Converter.Convert(obj, typeof(string), FilterColumnInfo.ConverterParameter, FilterColumnInfo.ConverterCultureInfo).ToString();
            else
            {
                if (obj == null)
                    return null;
                return obj.ToString();
            }

        }

        void filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var list = DistinctPropertyValues.Where(i => i.IsChecked).ToList();
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in DistinctPropertyValues.Where(i => i.IsChecked))
                    sb.AppendFormat("{0}{1}", sb.Length > 0 ? "," : "", i);

                FilterText = sb.ToString();
            }
            else
            {
                FilterText = null;
            }
            OnPropertyChanged("FilterReadOnly");
            OnPropertyChanged("FilterBackGround");
            OnPropertyChanged("FilterOperationsEnabled");
            OnPropertyChanged("SelectedFilterOperation");
        }

        #region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText = ((DatePicker)sender).SelectedDate.ToString();
        }

        private void DatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            FilterText = ((DatePicker)sender).SelectedDate.ToString();
        }
    }
}
