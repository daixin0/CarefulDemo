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
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Careful.Controls.Common;
using Careful.Controls.ToggleExtendControl;
using Careful.Core.Mvvm.Command;
using Careful.Core.Extensions;
using System.Reflection;
using System.Linq.Expressions;
using Careful.Core.Mvvm.PropertyChanged;

namespace Careful.Controls.DataGridExtendControl
{
    public delegate void CheckedSelectedChangedEvent(object sender, CheckSelectedEventArgs e);
    public enum TemplateColumnType
    {
        HyperLink,
        TextBlock,
        TextBox,
        CheckBox,
        MultiComboBox,
        ComboBox
    }
    public class FilterKeyValue : Selected
    {
        private string _id;

        /// <summary>
        /// Get or set ID value
        /// </summary>
        public string ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private string _diaplayValue;

        /// <summary>
        /// Get or set DiaplayValue value
        /// </summary>
        public string DiaplayValue
        {
            get { return _diaplayValue; }
            set { Set(ref _diaplayValue, value); }
        }

    }
    public class DataGridColumnHeaderFilter : Control
    {

        public DataGridColumnHeaderFilter()
        {
            ClearFilterCommand = new RelayCommand(ClearFilterFunction);
            FilterCommand = new RelayCommand(FilterFunction);
            FilterLoadCommand = new RelayCommand(FilterLoadFunction);
            SearchFilterCateoryCommand = new RelayCommand(SearchFilterCateoryFunction);
            ColumnLoadCommand = new RelayCommand(ColumnLoad);
        }

        public Type FilterType { get; set; }

        public bool IsVisiable
        {
            get { return (bool)GetValue(IsVisiableProperty); }
            set { SetValue(IsVisiableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisiable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisiableProperty =
            DependencyProperty.Register("IsVisiable", typeof(bool), typeof(DataGridColumnHeaderFilter));




        public ObservableCollection<FilterKeyValue> FilteredItemsSource
        {
            get { return (ObservableCollection<FilterKeyValue>)GetValue(FilteredItemsSourceProperty); }
            set { SetValue(FilteredItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilteredItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilteredItemsSourceProperty =
            DependencyProperty.Register("FilteredItemsSource", typeof(ObservableCollection<FilterKeyValue>), typeof(DataGridColumnHeaderFilter));



        #region Class Filter

        public ICommand ColumnLoadCommand
        {
            get { return (ICommand)GetValue(ColumnLoadCommandProperty); }
            set { SetValue(ColumnLoadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnLoadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnLoadCommandProperty =
            DependencyProperty.Register("ColumnLoadCommand", typeof(ICommand), typeof(DataGridColumnHeaderFilter));




        public ICommand FilterLoadCommand
        {
            get { return (ICommand)GetValue(FilterLoadCommandProperty); }
            set { SetValue(FilterLoadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterLoadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterLoadCommandProperty =
            DependencyProperty.Register("FilterLoadCommand", typeof(ICommand), typeof(DataGridColumnHeaderFilter));


        public ICommand SearchFilterCateoryCommand
        {
            get { return (ICommand)GetValue(SearchFilterCateoryCommandProperty); }
            set { SetValue(SearchFilterCateoryCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchFilterCateory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchFilterCateoryCommandProperty =
            DependencyProperty.Register("SearchFilterCateoryCommand", typeof(ICommand), typeof(DataGridColumnHeaderFilter));

        public IEnumerable CategoryFilter
        {
            get { return (IEnumerable)GetValue(CategoryFilterProperty); }
            set { SetValue(CategoryFilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryFilter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryFilterProperty =
            DependencyProperty.Register("CategoryFilter", typeof(IEnumerable), typeof(DataGridColumnHeaderFilter));



        public ICommand ClearFilterCommand
        {
            get { return (ICommand)GetValue(ClearFilterCommandProperty); }
            set { SetValue(ClearFilterCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClearFilterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearFilterCommandProperty =
            DependencyProperty.Register("ClearFilterCommand", typeof(ICommand), typeof(DataGridColumnHeaderFilter));

        public ICommand FilterCommand
        {
            get { return (ICommand)GetValue(FilterCommandProperty); }
            set { SetValue(FilterCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterCommandProperty =
            DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(DataGridColumnHeaderFilter));


        private void ColumnLoad(object args)
        {
            RoutedEventArgs routedEventArgs = args as RoutedEventArgs;
            DataGridColumnHeader columnHeader = (routedEventArgs.OriginalSource as Grid).GetParent<DataGridColumnHeader>();


        }


        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler FilterFinsh
        {
            add { AddHandler(FilterFinshEvent, value); }
            remove { RemoveHandler(FilterFinshEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent FilterFinshEvent = EventManager.RegisterRoutedEvent(
            "FilterFinsh", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridColumnHeaderFilter));



        private void FilterLoadFunction(object args)
        {
            RoutedEventArgs routedEventArgs = args as RoutedEventArgs;
            DataGridColumnHeader columnHeader = (routedEventArgs.OriginalSource as ToggleExtend).GetParent<DataGridColumnHeader>();
            DataGridColumn dataGridColumn = columnHeader.Column;
            OptionColumnInfo = new OptionColumnInfo(dataGridColumn, DataGridExtend);
            
            RoutedEventArgs routedEvent = new RoutedEventArgs(DataGridExtend.InitFilterColumnEvent, OptionColumnInfo.PropertyPath);
            DataGridExtend.RaiseEvent(routedEvent);

            ObservableCollection<FilterKeyValue> filter = new ObservableCollection<FilterKeyValue>();
            if (FilteredItemsSource == null)
            {
                FilteredItemsSource = new ObservableCollection<FilterKeyValue>();
                foreach (var item in DataGridExtend.FilteredItemsSource)
                {
                    if (item is ICateoryFilter cateory)
                    {
                        FilterKeyValue filterKeyValue = new FilterKeyValue();
                        filterKeyValue.DiaplayValue = cateory.FilterColumnValue?.ToString();
                        filterKeyValue.ID = cateory.ID;
                        FilteredItemsSource.Add(filterKeyValue);
                    }
                }
            }
            else
            {
                foreach (var item in DataGridExtend.FilteredItemsSource)
                {
                    if (item is ICateoryFilter cateory)
                    {
                        if (FilteredItemsSource.Where(p => p.ID == cateory.ID).Count() > 0)
                        {
                            FilterKeyValue filterKeyValue = FilteredItemsSource.Where(p => p.ID == cateory.ID).ToList()[0];
                            filterKeyValue.DiaplayValue = cateory.FilterColumnValue?.ToString();
                        }
                        else
                        {
                            FilterKeyValue filterKeyValue = new FilterKeyValue();
                            filterKeyValue.DiaplayValue = cateory.FilterColumnValue?.ToString();
                            FilteredItemsSource.Add(filterKeyValue);

                        }
                       
                    }
                }
                
            }
                

            foreach (var item in FilteredItemsSource)
            {
                if (filter.Where(p => p.DiaplayValue == item.DiaplayValue).ToList().Count <= 0)
                {
                    filter.Add(item);
                }
            }
            CategoryFilter = filter;
        }

        private void SearchFilterCateoryFunction(object obj)
        {
            ObservableCollection<FilterKeyValue> filter = new ObservableCollection<FilterKeyValue>();
            foreach (var item in FilteredItemsSource)
            {
                if (filter.Where(p => p.DiaplayValue == item.DiaplayValue).ToList().Count <= 0)
                {
                    filter.Add(item);
                }

            }
            CategoryFilter = filter.Where(p=>p.DiaplayValue.Contains(obj?.ToString()));
        }



        private void ClearFilterFunction(object args)
        {
            foreach (var item in FilteredItemsSource)
            {

                if (item.IsSelected)
                {
                    item.IsSelected = false;
                }
            }
            Predicate = null;
            RoutedEventArgs routedEvent = new RoutedEventArgs(DataGridColumnHeaderFilter.FilterFinshEvent, Predicate);
            this.RaiseEvent(routedEvent);
        }

        public Predicate<object> GeneratePredicate(string filterValue)
        {
            Predicate<object> predicate = GenerateFilterPredicate(OptionColumnInfo.PropertyPath, filterValue, FilterType, OptionColumnInfo.PropertyType.UnderlyingSystemType, new FilterOperationItem(Enums.FilterOperation.Contains, "Contains", "/Jib.WPF.Controls;component/Images/Contains.png"));

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

                default:
                    throw new ArgumentException("Could not decode Search Mode.  Did you add a new value to the enum, or send in Unknown?");
            }

        }
        public Predicate<object> Predicate { get; set; }

        OptionColumnInfo OptionColumnInfo { get; set; }

        public DataGridExtend DataGridExtend { get; set; }

        private Dictionary<string, Predicate<object>> keyValuePairs = new Dictionary<string, Predicate<object>>();
        private void FilterFunction(object args)
        {
            RoutedEventArgs routedEventArgs = args as RoutedEventArgs;
            DataGridColumnHeader columnHeader = (routedEventArgs.OriginalSource as Button).GetParent<DataGridColumnHeader>();
            Predicate<object> predicate = null;
            foreach (var item in FilteredItemsSource)
            {

                if (item.IsSelected)
                {
                    
                    if (predicate == null)
                        predicate = GeneratePredicate(item.DiaplayValue.ToString());
                    else
                        predicate = predicate.Or(GeneratePredicate(item.DiaplayValue.ToString()));


                }
            }

            if (!keyValuePairs.ContainsKey(columnHeader.Column.SortMemberPath))
            {
                keyValuePairs.Add(columnHeader.Column.SortMemberPath, predicate);
            }
            else
            {
                keyValuePairs.Remove(columnHeader.Column.SortMemberPath);
                keyValuePairs.Add(columnHeader.Column.SortMemberPath, predicate);
            }
            Predicate = null;
            foreach (var item in keyValuePairs)
            {
                if (Predicate == null)
                    Predicate = item.Value;
                else
                    Predicate.Or(item.Value);
            }
            RoutedEventArgs routedEvent = new RoutedEventArgs(DataGridColumnHeaderFilter.FilterFinshEvent, Predicate);
            this.RaiseEvent(routedEvent);

        }




        #endregion
    }
    public partial class DataGridExtend : DataGrid
    {
        public DataGridExtend()
        {
            Filters = new List<ColumnFilterControl>();
            _filterHandler = new PropertyChangedEventHandler(filter_PropertyChanged);

        }


        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler InitFilterColumn
        {
            add { AddHandler(InitFilterColumnEvent, value); }
            remove { RemoveHandler(InitFilterColumnEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent InitFilterColumnEvent = EventManager.RegisterRoutedEvent(
            "InitFilterColumn", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridExtend));



        public ICommand ColumnHeaderInitCommand
        {
            get { return (ICommand)GetValue(ColumnHeaderInitCommandProperty); }
            set { SetValue(ColumnHeaderInitCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnHeaderInitCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnHeaderInitCommandProperty =
            DependencyProperty.Register("ColumnHeaderInitCommand", typeof(ICommand), typeof(DataGridExtend));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Columns.Count > 0)
            {
                if (this.Columns[0].Header is ToggleExtend)
                {
                    ToggleExtend btnFilter = this.Columns[0].Header as ToggleExtend;
                    btnFilter.Checked -= BtnFilter_Checked;
                    btnFilter.Unchecked -= BtnFilter_Unchecked;
                    btnFilter.Checked += BtnFilter_Checked;
                    btnFilter.Unchecked += BtnFilter_Unchecked;
                }
                ColumnHeaderInitCommand = new RelayCommand(InitColumnHeader);
                DataGridColumnHeaders = new List<DataGridColumnHeader>();

            }
        }
        public List<DataGridColumnHeader> DataGridColumnHeaders { get; set; }

        private void InitColumnHeader(object args)
        {
            RoutedEventArgs routedEventArgs = args as RoutedEventArgs;
            DataGridColumnHeader columnHeader = (routedEventArgs.OriginalSource as Grid).GetParent<DataGridColumnHeader>();
            if (columnHeader == null || columnHeader.Column == null)
                return;
            foreach (var column in this.Columns)
            {
                if (columnHeader.Column == column)
                {
                    bool isFilter = (bool)column.GetValue(ColumnConfiguration.CanClassFilterProperty);
                    DataGridColumnHeaderFilter headerFilter = new DataGridColumnHeaderFilter();
                    headerFilter.DataGridExtend = this;
                    headerFilter.FilterType = FilterType;

                    headerFilter.FilterFinsh += HeaderFilter_FilterFinsh;
                    headerFilter.IsVisiable = isFilter;
                    columnHeader.Tag = headerFilter;
                    DataGridColumnHeaders.Add(columnHeader);
                }
            }

        }

        private void HeaderFilter_FilterFinsh(object sender, RoutedEventArgs e)
        {
            Predicate<object> predicate = null;
            foreach (var item in DataGridColumnHeaders)
            {
                DataGridColumnHeaderFilter dataGridColumnHeaderFilter = item.Tag as DataGridColumnHeaderFilter;
                if (dataGridColumnHeaderFilter.Predicate == null)
                    continue;
                if (predicate == null)
                    predicate = dataGridColumnHeaderFilter.Predicate;
                else
                {
                    predicate = predicate.Or(dataGridColumnHeaderFilter.Predicate);
                }
            }
            ListCollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource) as ListCollectionView;
            if (view != null && view.IsEditingItem)
                view.CommitEdit();
            if (view != null && view.IsAddingNew)
                view.CommitNew();
            if (CollectionView != null)
            {
                try
                {
                    CollectionView.Filter = predicate;
                }
                catch
                {

                }
            }
        }

        private void BtnFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            CanUserFilter = false;
        }

        private void BtnFilter_Checked(object sender, RoutedEventArgs e)
        {
            CanUserFilter = true;
        }

        #region Filter Column

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("False")]
        public bool CanUserSelectDistinct
        {
            get { return (bool)GetValue(CanUserSelectDistinctProperty); }
            set
            {
                foreach (var optionControl in Filters)
                    optionControl.CanUserSelectDistinct = value;
                SetValue(CanUserSelectDistinctProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CanUserSelectDistinct.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUserSelectDistinctProperty =
            DependencyProperty.Register("CanUserSelectDistinct", typeof(bool), typeof(DataGridExtend));



        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("True")]
        public bool CanUserFilter
        {
            get { return (bool)GetValue(CanUserFilterProperty); }
            set { SetValue(CanUserFilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanUserFilter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUserFilterProperty =
            DependencyProperty.Register("CanUserFilter", typeof(bool), typeof(DataGridExtend));

        public Type FilterType { get; set; }


        public ICollectionView CollectionView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(this.ItemsSource);
            }
        }
        public List<ColumnFilterControl> Filters
        {
            get { return (List<ColumnFilterControl>)GetValue(FiltersProperty); }
            set { SetValue(FiltersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Filters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiltersProperty =
            DependencyProperty.Register("Filters", typeof(List<ColumnFilterControl>), typeof(DataGridExtend));



        public static readonly DependencyProperty FilteredItemsSourceProperty =
                                                               DependencyProperty.Register("FilteredItemsSource", typeof(IEnumerable), typeof(DataGridExtend),
                                                               new PropertyMetadata(null, new PropertyChangedCallback(OnFilteredItemsSourceChanged)));

        public IEnumerable FilteredItemsSource
        {
            get { return (IEnumerable)GetValue(FilteredItemsSourceProperty); }
            set { SetValue(FilteredItemsSourceProperty, value); }
        }
        public static event EventHandler DataLoadFinshEvent;
        public static void OnFilteredItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataGridExtend g = sender as DataGridExtend;
            if (g != null)
            {
                try
                {
                    //if (e.NewValue == null)
                    //    return;
                    Type srcT = e.NewValue.GetType().GetInterfaces().First(i => i.Name.StartsWith("IEnumerable"));
                    g.FilterType = srcT.GetGenericArguments().First();
                    if (g.ItemsSource == null)
                        g.ItemsSource = (IEnumerable)e.NewValue;
                    //g.Filters = new List<ColumnFilterControl>();
                    g.CanUserFilter = false;
                    if (g.Filters != null)
                        foreach (var filter in g.Filters)
                            filter.ResetControl();

                    if (DataLoadFinshEvent != null)
                    {
                        DataLoadFinshEvent(null, null);
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }
        void filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilterChanged")
            {
                ColumnFilterControl cf = sender as ColumnFilterControl;

                Predicate<object> predicate = null;

                foreach (var filter in Filters)
                {
                    if (filter.HasPredicate)
                        if (predicate == null)
                            predicate = filter.GeneratePredicate();
                        else
                            predicate = predicate.And(filter.GeneratePredicate());
                    if (cf.FilterText == null)
                    {
                        if (predicate == null)
                            predicate = filter.GeneratePredicate();
                        else
                            predicate = predicate.And(filter.GeneratePredicate() == null ? predicate : filter.GeneratePredicate());
                    }
                }
                //bool canContinue = true;
                //var args = new CancelableFilterChangedEventArgs(predicate);
                //if (BeforeFilterChanged != null && !IsResetting)
                //{
                //    BeforeFilterChanged(this, args);
                //    canContinue = !args.Cancel;
                //}
                //if (canContinue)
                //{
                ListCollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource) as ListCollectionView;
                if (view != null && view.IsEditingItem)
                    view.CommitEdit();
                if (view != null && view.IsAddingNew)
                    view.CommitNew();
                if (CollectionView != null)
                {
                    try
                    {
                        CollectionView.Filter = predicate;
                    }
                    catch
                    {

                    }

                    //int count = 1;
                    ObservableCollection<ISelected> list = new ObservableCollection<ISelected>();

                    foreach (var item in CollectionView)
                    {
                        ISelected selectedModel = item as ISelected;
                        //selectedModel.Num= count++;
                        list.Add(selectedModel);
                    }
                    //foreach (var item in list)
                    //{
                    //    if (item == null)
                    //        continue;
                    //    item.Num = count++;
                    //}
                    if (list != null && list.Count > 0)
                    {
                        //FilterFinshItemsSource = list;

                        //if (AfterFilterChanged != null)
                        //    AfterFilterChanged(this, new FilterChangedEventArgs(predicate));
                    }

                }


                //}
                //else
                //{
                //    IsResetting = true;
                //    var ctrl = sender as ColumnFilterControl;
                //    ctrl.ResetControl();
                //    IsResetting = false;
                //}
            }
        }
        private PropertyChangedEventHandler _filterHandler;
        internal void RegisterOptionControl(ColumnFilterControl ctrl)
        {
            if (!Filters.Contains(ctrl))
            {
                ctrl.PropertyChanged += _filterHandler;
                Filters.Add(ctrl);
            }
        }

        #endregion


        #region Freeze Column

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("False")]
        public bool CanUserFreeze
        {
            get { return (bool)GetValue(CanUserFreezeProperty); }
            set
            {
                foreach (var optionControl in Filters)
                    optionControl.CanUserFreeze = value;
                SetValue(CanUserFreezeProperty, value);

            }
        }

        // Using a DependencyProperty as the backing store for CanUserFreeze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUserFreezeProperty =
            DependencyProperty.Register("CanUserFreeze", typeof(bool), typeof(DataGridExtend));

        public void UnFreezeColumn(DataGridColumn column)
        {
            if (this.FrozenColumnCount > 0 && column.IsFrozen && this.Columns != null && this.Columns.Contains(column))
            {
                this.FrozenColumnCount--;
                column.DisplayIndex = this.FrozenColumnCount;
            }
        }

        public void FreezeColumn(DataGridColumn column)
        {
            if (this.Columns != null && this.Columns.Contains(column))
            {
                column.DisplayIndex = this.FrozenColumnCount;
                this.FrozenColumnCount++;
            }
        }
        public bool IsFrozenColumn(DataGridColumn column)
        {
            if (this.Columns != null && this.Columns.Contains(column))
            {
                return column.DisplayIndex < this.FrozenColumnCount;
            }
            else
            {
                return false;
            }
        }

        #endregion

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("False")]
        public bool CanUserGroup
        {
            get { return (bool)GetValue(CanUserGroupProperty); }
            set
            {
                foreach (var optionControl in Filters)
                    optionControl.CanUserGroup = value;
                SetValue(CanUserGroupProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CanUserGroup.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUserGroupProperty =
            DependencyProperty.Register("CanUserGroup", typeof(bool), typeof(DataGridExtend));


        public bool IsGrouped(string boundPropertyName)
        {
            if (CollectionView != null && CollectionView.Groups != null)
            {
                foreach (var g in CollectionView.GroupDescriptions)
                {
                    var pgd = g as PropertyGroupDescription;

                    if (pgd != null)
                        if (pgd.PropertyName == boundPropertyName)
                            return true;
                }
            }

            return false;
        }
        public void AddGroup(string boundPropertyName)
        {
            if (!string.IsNullOrWhiteSpace(boundPropertyName) && CollectionView != null && CollectionView.GroupDescriptions != null)
            {
                foreach (var groupedCol in CollectionView.GroupDescriptions)
                {
                    var propertyGroup = groupedCol as PropertyGroupDescription;

                    if (propertyGroup != null && propertyGroup.PropertyName == boundPropertyName)
                        return;
                }

                CollectionView.GroupDescriptions.Add(new PropertyGroupDescription(boundPropertyName));
            }
        }
        public void RemoveGroup(string boundPropertyName)
        {
            if (!string.IsNullOrWhiteSpace(boundPropertyName) && CollectionView != null && CollectionView.GroupDescriptions != null)
            {
                PropertyGroupDescription selectedGroup = null;

                foreach (var groupedCol in CollectionView.GroupDescriptions)
                {
                    var propertyGroup = groupedCol as PropertyGroupDescription;

                    if (propertyGroup != null && propertyGroup.PropertyName == boundPropertyName)
                    {
                        selectedGroup = propertyGroup;
                    }
                }

                if (selectedGroup != null)
                    CollectionView.GroupDescriptions.Remove(selectedGroup);

                //if (CollapseLastGroup && CollectionView.Groups != null)
                //foreach (CollectionViewGroup group in CollectionView.Groups)
                //    RecursiveCollapse(group);
            }
        }


        #region Check All

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (this.CurrentCell.Column == null)
                return;
            if (this.CurrentCell.Column.Header is CheckBox)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    foreach (var item in this.SelectedItems)
                    {
                        ISelected selectedModel = item as ISelected;
                        CheckBox cb = (Mouse.DirectlyOver as UIElement) as CheckBox;
                        if (cb == null || cb.DataContext != selectedModel)
                        {
                            selectedModel.IsSelected = true;
                        }

                    }
                }

            }

        }

        public bool? AllChecked
        {
            get { return (bool?)GetValue(AllCheckedProperty); }
            set { SetValue(AllCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllCheckedProperty =
            DependencyProperty.Register("AllChecked", typeof(bool?), typeof(DataGridExtend), new PropertyMetadata(true));



        public event CheckedSelectedChangedEvent CheckedSelectedChanged;

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this.Columns.Count > 0)
            {
                CheckBox cbAllCheck = null;
                if (this.Columns[0].Header is CheckBox)
                {
                    cbAllCheck = this.Columns[0].Header as CheckBox;
                }
                else if (this.Columns[1].Header is CheckBox)
                {
                    cbAllCheck = this.Columns[1].Header as CheckBox;
                }
                if (cbAllCheck != null)
                {
                    BindingOperations.SetBinding(cbAllCheck, CheckBox.IsCheckedProperty, new Binding()
                    {
                        Path = new PropertyPath("AllChecked"),
                        Source = this,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

                    cbAllCheck.Click -= CbAllCheck_Click;
                    cbAllCheck.Click += CbAllCheck_Click;
                    if (this.Items.Count > 0)
                    {
                        //AllChecked = true;
                        bool isExistSelect = false;
                        bool isExistNoSelect = false;
                        foreach (var item in ItemsSource)
                        {
                            ISelected selectedModel = item as ISelected;
                            if (selectedModel == null)
                                return;
                            selectedModel.PropertyChanged -= SelectedModel_PropertyChanged;
                            selectedModel.PropertyChanged += SelectedModel_PropertyChanged;
                            if (selectedModel.IsSelected)
                            {
                                isExistSelect = true;
                            }
                            if (!selectedModel.IsSelected)
                            {
                                isExistNoSelect = true;
                            }
                            //if (!selectedModel.IsSelected && AllChecked)
                            //{
                            //    AllChecked = false;
                            //}
                        }
                        if (isExistSelect && !isExistNoSelect)
                        {
                            AllChecked = true;
                        }
                        else if (!isExistSelect && isExistNoSelect)
                        {
                            AllChecked = false;
                        }
                        else if (isExistSelect && isExistNoSelect)
                        {
                            AllChecked = null;
                        }
                    }
                    else
                    {
                        AllChecked = false;
                    }
                }
            }
        }

        public bool IsDefaultSelectAll
        {
            get { return (bool)GetValue(IsDefaultSelectAllProperty); }
            set { SetValue(IsDefaultSelectAllProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDefaultSelectAll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDefaultSelectAllProperty =
            DependencyProperty.Register("IsDefaultSelectAll", typeof(bool), typeof(DataGridExtend));


        private void SelectedModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsSelected")
                return;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                return;
            ISelected selectedModel = sender as ISelected;
            if (selectedModel.IsSelected)
            {
                AllChecked = true;
                if (!IsDefaultSelectAll)
                {
                    foreach (var item in ItemsSource)
                    {
                        ISelected model = item as ISelected;
                        if (model == null)
                            continue;
                        if (!model.IsSelected)
                        {
                            AllChecked = null;
                            break;
                        }
                    }
                    if (CheckedSelectedChanged != null)
                    {
                        if (this.SelectedItem != null)
                            CheckedSelectedChanged(this.SelectedItem, new CheckSelectedEventArgs(IsDefaultSelectAll));
                        else
                            CheckedSelectedChanged(sender, new CheckSelectedEventArgs(IsDefaultSelectAll));
                    }
                }
            }
            else
            {
                AllChecked = false;
                foreach (var item in ItemsSource)
                {
                    ISelected model = item as ISelected;
                    if (model == null)
                        continue;
                    if (model.IsSelected)
                    {
                        AllChecked = null;
                        break;
                    }
                }
                if (!IsDefaultSelectAll)
                {
                    if (CheckedSelectedChanged != null)
                    {
                        if (this.SelectedItem != null)
                            CheckedSelectedChanged(this.SelectedItem, new CheckSelectedEventArgs(IsDefaultSelectAll));
                        else
                            CheckedSelectedChanged(sender, new CheckSelectedEventArgs(IsDefaultSelectAll));
                    }
                }
            }
        }
        private void CbAllCheck_Click(object sender, RoutedEventArgs e)
        {
            IsDefaultSelectAll = true;
            foreach (var item in ItemsSource)
            {
                ISelected selectedModel = item as ISelected;
                if (selectedModel == null)
                    continue;
                if (AllChecked == true)
                {
                    if (!selectedModel.IsSelected)
                        selectedModel.IsSelected = true;
                }
                else
                {
                    if (selectedModel.IsSelected)
                        selectedModel.IsSelected = false;
                }
            }
            if (CheckedSelectedChanged != null)
            {
                CheckedSelectedChanged(null, new CheckSelectedEventArgs(true));
            }
            IsDefaultSelectAll = false;
        }
        #endregion
    }

}
