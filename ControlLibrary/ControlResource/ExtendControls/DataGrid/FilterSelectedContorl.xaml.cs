using ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ControlResource.ExtendControlStyle.DataGrid.Enums;

namespace ControlResource.ExtendControlStyle.DataGrid
{
    /// <summary>
    /// FilterSelectedContorl.xaml 的交互逻辑
    /// </summary>
    public partial class FilterSelectedContorl : UserControl, INotifyPropertyChanged
    {

        #region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion
        public FilterSelectedContorl()
        {
            InitializeComponent();
            
            
        }
        
        public FilterModel FilterSelectedItem
        {
            get { return (FilterModel)GetValue(FilterSelectedItemProperty); }
            set { SetValue(FilterSelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterSelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterSelectedItemProperty =
            DependencyProperty.Register("FilterSelectedItem", typeof(FilterModel), typeof(FilterSelectedContorl));
        
        public OptionColumnInfo FilterColumnInfo { get; set; }

        private Func<object, object> _boundColumnPropertyAccessor = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in FilterSelectedItem.Grid.Columns)
            {
                if (item.Visibility == Visibility.Collapsed)
                    continue;
                Binding BindingSelected = null;
                string StringPath = "";
                if (item is DataGridTemplateColumn)
                {
                    var boundColumn = item as DataGridTemplateColumn;
                    if (boundColumn != null)
                    {
                        DataTemplate dt = boundColumn.CellTemplate as DataTemplate;
                        if (dt != null)
                        {

                            if (dt.DataType != null)
                            {
                                string dataType = dt.DataType.ToString();
                                TemplateColumnType templateType = (TemplateColumnType)Enum.Parse(typeof(TemplateColumnType), dataType);

                                switch (templateType)
                                {
                                    case TemplateColumnType.HyperLink:
                                        StackPanel sp = dt.LoadContent() as StackPanel;
                                        TextBlock tb = sp.Children[3] as TextBlock;
                                        InlineUIContainer il = tb.Inlines.FirstInline as InlineUIContainer;
                                        Label lbl = il.Child as Label;
                                        Run r = lbl.Content as Run;
                                        BindingSelected = BindingOperations.GetBinding(r, Run.TextProperty);
                                        break;
                                    case TemplateColumnType.TextBlock:
                                        TextBlock tb1 = dt.LoadContent() as TextBlock;
                                        BindingSelected = BindingOperations.GetBinding(tb1, TextBlock.TextProperty);
                                        break;
                                    case TemplateColumnType.CheckBox:

                                        CheckBox cb = new CheckBox();
                                        if (dt.LoadContent() is CheckBox)
                                        {
                                            cb = dt.LoadContent() as CheckBox;

                                        }
                                        else if (dt.LoadContent() is Grid)
                                        {
                                            Grid gd1 = dt.LoadContent() as Grid;
                                            if (gd1.Children.Count > 1)
                                            {
                                                cb = gd1.Children[1] as CheckBox;
                                            }
                                        }
                                        if (cb.IsThreeState)
                                        {
                                            //IsSpecialState = true;
                                            BindingSelected = BindingOperations.GetBinding(cb, CheckBox.ContentProperty);
                                        }
                                        else
                                        {
                                            BindingSelected = BindingOperations.GetBinding(cb, CheckBox.IsCheckedProperty);

                                        }
                                        break;
                                    case TemplateColumnType.MultiComboBox:
                                        try
                                        {
                                            MultiComboBox.MultiComboBox multiCbb = dt.LoadContent() as MultiComboBox.MultiComboBox;
                                            BindingSelected = BindingOperations.GetBinding(multiCbb, MultiComboBox.MultiComboBox.TextProperty);
                                        }
                                        catch (Exception ex)
                                        {
                                            BindingSelected = null;
                                        }
                                        break;
                                    case TemplateColumnType.ComboBox:
                                        Grid gd = dt.LoadContent() as Grid;
                                        ComboBox com = gd.Children[0] as ComboBox;
                                        if (!string.IsNullOrWhiteSpace(com.DisplayMemberPath))
                                        {
                                            BindingSelected = BindingOperations.GetBinding(com, ComboBox.SelectedItemProperty);
                                            StringPath = com.DisplayMemberPath;
                                        }
                                        else
                                        {
                                            BindingSelected = BindingOperations.GetBinding(com, ComboBox.SelectedItemProperty);
                                        }

                                        break;
                                }
                            }

                        }

                    }
                }
                else
                {
                    var boundColumn = item as DataGridBoundColumn;
                    if (boundColumn != null)
                    {
                        BindingSelected = boundColumn.Binding as System.Windows.Data.Binding;
                    }
                }
                if (BindingSelected != null && BindingSelected.Path != null && !string.IsNullOrWhiteSpace(BindingSelected.Path.Path))
                {
                    System.Reflection.PropertyInfo propInfo = null;
                    if (!string.IsNullOrWhiteSpace(StringPath))
                    {
                        System.Reflection.PropertyInfo propInfo2 = null;
                        if (FilterSelectedItem.Grid.FilterType != null)
                            propInfo2 = FilterSelectedItem.Grid.FilterType.GetProperty(BindingSelected.Path.Path);
                        propInfo = propInfo2.PropertyType.GetProperty(StringPath);
                        FilterSelectedItem.GridColumnsName.Add(new ColumnInfo() { ColumnTitle = item.Header.ToString(), ColumnField =  BindingSelected.Path.Path + "." + StringPath, ColumnsType = propInfo != null ? propInfo.PropertyType : typeof(string), DataGridColumn = item });
                    }
                    else
                    {
                        if (FilterSelectedItem.Grid.FilterType != null)
                            propInfo = FilterSelectedItem.Grid.FilterType.GetProperty(BindingSelected.Path.Path);
                        string title = "";
                        if(item.Header is CheckBox)
                        {
                            CheckBox cb = item.Header as CheckBox;
                            title = cb.Content == null ? "全选" : cb.Content.ToString();
                        }
                        else
                        {
                            title = item.Header.ToString();
                        }
                        FilterSelectedItem.GridColumnsName.Add(new ColumnInfo() { ColumnTitle = title, ColumnField = BindingSelected.Path.Path, ColumnsType = propInfo != null ? propInfo.PropertyType : typeof(string), DataGridColumn = item });
                    }
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColumnInfo info = cbColumns.SelectedItem as ColumnInfo;
            if (FilterSelectedItem.Grid.FilterType == null || info == null)
                return;

            FilterColumnInfo = new OptionColumnInfo(info.DataGridColumn, FilterSelectedItem.Grid);
            FilterSelectedItem.FilterOperations.Clear();
            if (FilterColumnInfo.PropertyType != null)
            {
                if (FilterColumnInfo.PropertyConvertType != null)
                {
                    if (TypeHelper.IsStringType(FilterColumnInfo.PropertyConvertType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "等于", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Contains, "包含", "/Jib.WPF.Controls;component/Images/Contains.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.StartsWith, "开始于", "/Jib.WPF.Controls;component/Images/StartsWith.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.EndsWith, "结束于", "/Jib.WPF.Controls;component/Images/EndsWith.png"));
                        CalcControlVisibility(ColumnType.String);
                    }
                    if (TypeHelper.IsNumbericType(FilterColumnInfo.PropertyConvertType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThan, ">", "/Jib.WPF.Controls;component/Images/GreaterThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThanEqual, ">=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThan, "<", "/Jib.WPF.Controls;component/Images/LessThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThanEqual, "<=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.NotEquals, "!=", "/Jib.WPF.Controls;component/Images/NotEqual.png"));
                        CalcControlVisibility(ColumnType.Num);
                    }
                    if (TypeHelper.IsBoolType(FilterColumnInfo.PropertyConvertType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        CalcControlVisibility(ColumnType.Bool);
                    }
                    if (TypeHelper.IsDateTimeType(FilterColumnInfo.PropertyConvertType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThan, ">", "/Jib.WPF.Controls;component/Images/GreaterThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThanEqual, ">=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThan, "<", "/Jib.WPF.Controls;component/Images/LessThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThanEqual, "<=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.NotEquals, "!=", "/Jib.WPF.Controls;component/Images/NotEqual.png"));
                        CalcControlVisibility(ColumnType.Date);
                    }
                }
                else
                {
                    if (TypeHelper.IsStringType(FilterColumnInfo.PropertyType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "等于", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Contains, "包含", "/Jib.WPF.Controls;component/Images/Contains.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.StartsWith, "开始于", "/Jib.WPF.Controls;component/Images/StartsWith.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.EndsWith, "结束于", "/Jib.WPF.Controls;component/Images/EndsWith.png"));
                        CalcControlVisibility(ColumnType.String);
                    }
                    if (TypeHelper.IsNumbericType(FilterColumnInfo.PropertyType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThan, ">", "/Jib.WPF.Controls;component/Images/GreaterThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThanEqual, ">=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThan, "<", "/Jib.WPF.Controls;component/Images/LessThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThanEqual, "<=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.NotEquals, "!=", "/Jib.WPF.Controls;component/Images/NotEqual.png"));
                        CalcControlVisibility(ColumnType.Num);
                    }
                    if (TypeHelper.IsBoolType(FilterColumnInfo.PropertyType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        CalcControlVisibility(ColumnType.Bool);
                    }
                    if (TypeHelper.IsDateTimeType(FilterColumnInfo.PropertyType))
                    {
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.Equals, "=", "/Jib.WPF.Controls;component/Images/Equal.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThan, ">", "/Jib.WPF.Controls;component/Images/GreaterThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.GreaterThanEqual, ">=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThan, "<", "/Jib.WPF.Controls;component/Images/LessThan.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.LessThanEqual, "<=", "/Jib.WPF.Controls;component/Images/GreaterThanEqual.png"));
                        FilterSelectedItem.FilterOperations.Add(new FilterOperationItem(Enums.FilterOperation.NotEquals, "!=", "/Jib.WPF.Controls;component/Images/NotEqual.png"));
                        CalcControlVisibility(ColumnType.Date);
                    }
                }
               
            }
            if (FilterColumnInfo != null && FilterColumnInfo.IsValid)
            {
                FilterSelectedItem.BoolPropertyValues.Clear();
                _boundColumnPropertyAccessor = null;
                if (!string.IsNullOrWhiteSpace(FilterColumnInfo.PropertyPath))
                {
                    if (FilterColumnInfo.PropertyPath.Contains('.'))
                        throw new ArgumentException(string.Format("This version of the grid does not support a nested property path such as '{0}'.  Please make a first-level property for filtering and bind to that.", FilterColumnInfo.PropertyPath));

                    ParameterExpression arg = System.Linq.Expressions.Expression.Parameter(typeof(object), "x");
                    Type type = FilterSelectedItem.Grid.FilterType;
                    System.Linq.Expressions.Expression expr = System.Linq.Expressions.Expression.Convert(arg, type);

                    expr = System.Linq.Expressions.Expression.Property(expr, type, FilterColumnInfo.PropertyPath);
                    if (!string.IsNullOrWhiteSpace(FilterColumnInfo.StringPath))
                    {
                        System.Reflection.PropertyInfo propInfo2 = FilterSelectedItem.Grid.FilterType.GetProperty(FilterColumnInfo.PropertyPath);
                        expr = System.Linq.Expressions.Expression.Property(expr, propInfo2.PropertyType, FilterColumnInfo.StringPath);
                    }
                    System.Linq.Expressions.Expression conversion = System.Linq.Expressions.Expression.Convert(expr, typeof(object));
                    _boundColumnPropertyAccessor = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(conversion, arg).Compile();

                    if (_boundColumnPropertyAccessor != null)
                    {
                        List<object> result = new List<object>();
                        foreach (var i in FilterSelectedItem.Grid.ItemsSource)
                        {
                            AllSelectedPropertyChanged selectedModel = i as AllSelectedPropertyChanged;
                            object value = _boundColumnPropertyAccessor(selectedModel);
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

                        //foreach (var item in result)
                        //{
                        //    FilterSelectedItem.ColumnValue = item.ToString() + ",";
                        //}
                        //if (!string.IsNullOrWhiteSpace(FilterSelectedItem.ColumnValue))
                        //{
                        //    FilterSelectedItem.ColumnValue = FilterSelectedItem.ColumnValue.Remove(FilterSelectedItem.ColumnValue.Length - 1);
                        //}

                    }
                }
            }
        }
        private void CalcControlVisibility(ColumnType type)
        {
            cbDistinctProperties.Visibility = System.Windows.Visibility.Collapsed;
            txtFilter.Visibility = System.Windows.Visibility.Collapsed;
            spDate.Visibility = Visibility.Collapsed;
            switch (type)
            {
                case ColumnType.String:
                    txtFilter.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ColumnType.Bool:
                    cbDistinctProperties.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ColumnType.Num:
                    txtFilter.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ColumnType.Date:
                    spDate.Visibility = Visibility.Visible;
                    break;

            }
        }
        private void cbDistinctProperties_DropDownOpened(object sender, EventArgs e)
        {
            if (_boundColumnPropertyAccessor != null)
            {
                if (FilterSelectedItem.BoolPropertyValues.Count == 0)
                {
                    List<object> result = new List<object>();
                    foreach (var i in FilterSelectedItem.Grid.ItemsSource)
                    {
                        AllSelectedPropertyChanged selectedModel = i as AllSelectedPropertyChanged;
                        object value = _boundColumnPropertyAccessor(selectedModel);
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
                            Tag = obj
                        };
                        FilterSelectedItem.BoolPropertyValues.Add(item);
                    }
                }
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

        private void cbDistinctProperties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterSelectedItem.BoolPropertyValueSelected != null)
            {
                if (FilterSelectedItem.BoolPropertyValueSelected.Tag != null)
                {
                    FilterSelectedItem.ColumnValue = FilterSelectedItem.BoolPropertyValueSelected.Tag.ToString();
                }
                else
                {
                    FilterSelectedItem.ColumnValue = null;
                }
            }
                
        }
    }
}
