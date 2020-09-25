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

namespace Careful.Controls.DataGridControl
{
    public delegate void FilterChangedEvent(object sender, FilterChangedEventArgs e);
    public delegate void CancelableFilterChangedEvent(object sender, CancelableFilterChangedEventArgs e);
    public delegate void CheckedSelectedChangedEvent(object sender, CheckSelectedEventArgs e);
    public enum TemplateColumnType
    {
        HyperLink,
        TextBlock,
        CheckBox,
        MultiComboBox,
        ComboBox
    }
    public partial class DataGridExtend : System.Windows.Controls.DataGrid, INotifyPropertyChanged
    {
        public event CancelableFilterChangedEvent BeforeFilterChanged;
        public event FilterChangedEvent AfterFilterChanged;

        private List<ColumnOptionControl> _optionControls = new List<ColumnOptionControl>();
        private PropertyChangedEventHandler _filterHandler;


        public DataGridExtend()
        {
            ResourceDictionary listboxStyle = new ResourceDictionary();
            listboxStyle.Source = new Uri("Careful.Controls;component/ExtendControlStyle/DataGrid/DataGridExtendStyle.xaml", UriKind.Relative);
            this.Style = (System.Windows.Style)listboxStyle["DataGridExtendStyle"];
           
            Filters = new List<ColumnFilterControl>();
            _filterHandler = new PropertyChangedEventHandler(filter_PropertyChanged);

            FilterDataViewModel.Instance = new FilterDataViewModel();
            

        }

        ToggleButton btnFilter = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Columns.Count > 0)
            {
                if (this.Columns[0].Header is ToggleButton&&!(this.Columns[0].Header is CheckBox))
                {
                    btnFilter = this.Columns[0].Header as ToggleButton;
                    if (!IsFilterWindow)
                    {
                        btnFilter.Click -= BtnFilter_Click;
                        btnFilter.Click += BtnFilter_Click;

                    }
                    else
                    {
                        btnFilter.Checked -= BtnFilter_Checked;
                        btnFilter.Unchecked -= BtnFilter_Unchecked;
                        btnFilter.Checked += BtnFilter_Checked;
                        btnFilter.Unchecked += BtnFilter_Unchecked;
                    }
                }
            }
        }
        FilterDataWindow window = null;
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterDataViewModel.Instance.Grid = this;
            FilterDataViewModel.Instance.ClearFilterEvent -= Instance_ClearFilterEvent;
            FilterDataViewModel.Instance.ClearFilterEvent += Instance_ClearFilterEvent;
            FilterDataViewModel.Instance.StartFilterEvent -= Instance_StartFilterEvent;
            FilterDataViewModel.Instance.StartFilterEvent += Instance_StartFilterEvent;
            window = new FilterDataWindow();
            window.DataContext = FilterDataViewModel.Instance;
            window.ShowInTaskbar = false;
            window.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(m => m.IsActive);
            window.ShowDialog();
        }

        private void Instance_StartFilterEvent(object sender, EventArgs e)
        {
            bool isExistData = false;
            foreach (var item in CollectionView)
            {
                isExistData = true;
            }
            if(isExistData)
                btnFilter.Content = "M697.974154 547.682462V984.615385a39.384615 39.384615 0 1 1-78.769231 0V533.267692a39.384615 39.384615 0 0 1 9.294769-25.363692l293.257846-347.687385C963.347692 112.088615 948.932923 78.769231 888.516923 78.769231H135.798154c-60.967385 0-75.224615 32.059077-34.658462 82.471384l277.188923 345.009231a39.384615 39.384615 0 0 1 8.664616 24.654769v311.611077c1.654154 1.496615 3.702154 3.150769 5.986461 5.041231 12.603077 9.924923 32.295385 23.473231 58.998154 40.172308a39.384615 39.384615 0 1 1-41.747692 66.717538c-76.484923-47.891692-102.006154-68.135385-102.006154-99.800615V544.768L39.699692 210.550154C-41.432615 109.725538 7.404308 0 135.798154 0h752.718769c128.866462 0 177.703385 113.270154 93.184 211.259077l-283.726769 336.344615zM787.534769 669.538462a39.384615 39.384615 0 0 1 0-78.769231h157.538462a39.384615 39.384615 0 0 1 0 78.769231h-157.538462z m0 157.538461a39.384615 39.384615 0 0 1 0-78.769231h236.307693a39.384615 39.384615 0 0 1 0 78.769231h-236.307693z";
        }

        private void Instance_ClearFilterEvent(object sender, EventArgs e)
        {
            btnFilter.Content = "M9.4229997,18.347 C14.338999,18.347 18.345999,14.339 18.345999,9.4229999 18.345999,4.508 14.338999,0.5 9.4229997,0.5 4.5079998,0.5 0.5,4.508 0.5,9.4229999 0.5,14.339 4.5079998,18.347 9.4229997,18.347 z M15.733999,15.734 L17.732999,17.733 M16.972,16.972 L18.5,18.5";
            CollectionView.Filter = null;
        }

        public bool IsVisiableSort
        {
            get { return (bool)GetValue(IsVisiableSortProperty); }
            set { SetValue(IsVisiableSortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisiableSort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisiableSortProperty =
            DependencyProperty.Register("IsVisiableSort", typeof(bool), typeof(DataGridExtend), new PropertyMetadata(true));


        public bool IsFilterWindow
        {
            get { return (bool)GetValue(IsFilterWindowProperty); }
            set { SetValue(IsFilterWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFilterWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFilterWindowProperty =
            DependencyProperty.Register("IsFilterWindow", typeof(bool), typeof(DataGridExtend));

        protected bool IsResetting { get; set; }

        public List<ColumnFilterControl> Filters
        {
            get { return (List<ColumnFilterControl>)GetValue(FiltersProperty); }
            set { SetValue(FiltersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Filters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiltersProperty =
            DependencyProperty.Register("Filters", typeof(List<ColumnFilterControl>), typeof(DataGridExtend));


        public Type FilterType { get; set; }
        private ICollectionView _collectionView;
        protected ICollectionView CollectionView
        {
            get {
                return CollectionViewSource.GetDefaultView(this.ItemsSource); }
            set { _collectionView = value; }
        }
        

        #region FilteredItemsSource DependencyProperty
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
                catch(Exception ex)
                {

                }
                
            }
        }
        #endregion
        
        
        public ObservableCollection<AllSelectedPropertyChanged> FilterFinshItemsSource
        {
            get { return (ObservableCollection<AllSelectedPropertyChanged>)GetValue(FilterFinshItemsSourceProperty); }
            set { SetValue(FilterFinshItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterFinshItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterFinshItemsSourceProperty =
            DependencyProperty.Register("FilterFinshItemsSource", typeof(ObservableCollection<AllSelectedPropertyChanged>), typeof(DataGridExtend),
                new PropertyMetadata(null, new PropertyChangedCallback(OnFilterFinshItemsSourceChanged)));

        public static void OnFilterFinshItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //ListCollectionView view = CollectionViewSource.GetDefaultView(e.NewValue) as ListCollectionView;
            //if (view != null && view.IsEditingItem)
            //    view.CommitEdit();
            //if (view != null && view.IsAddingNew)
            //    view.CommitNew();
            
        }
        
        public string SortPropertyName
        {
            get { return (string)GetValue(SortPropertyNameProperty); }
            set { SetValue(SortPropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SortPropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortPropertyNameProperty =
            DependencyProperty.Register("SortPropertyName", typeof(string), typeof(DataGridExtend), new PropertyMetadata(""));



        public bool? IsPositiveSequence
        {
            get { return (bool?)GetValue(IsPositiveSequenceProperty); }
            set { SetValue(IsPositiveSequenceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPositiveSequence.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPositiveSequenceProperty =
            DependencyProperty.Register("IsPositiveSequence", typeof(bool?), typeof(DataGridExtend));



        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if(e.Property.Name== "IsPositiveSequence")
            {
                this.CollectionView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription();
                sd.PropertyName = SortPropertyName;
                if (IsPositiveSequence == true)
                {
                    sd.Direction = ListSortDirection.Ascending;
                }else if (IsPositiveSequence == false)
                {
                    sd.Direction = ListSortDirection.Descending;
                }
                this.CollectionView.SortDescriptions.Add(sd);
                ObservableCollection<AllSelectedPropertyChanged> list = new ObservableCollection<AllSelectedPropertyChanged>();

                foreach (var item in CollectionView)
                {
                    AllSelectedPropertyChanged selectedModel = item as AllSelectedPropertyChanged;
                    //selectedModel.Num= count++;
                    list.Add(selectedModel);
                }
                int count = 1;
                foreach (var item in list)
                {
                    if (item == null)
                        continue;
                    item.Num = count++;
                }

            }
            
        }

        internal void RegisterOptionWindow()
        {
            FilterDataViewModel.Instance.PropertyChanged -= _filterHandler;
            FilterDataViewModel.Instance.PropertyChanged += _filterHandler;
        }

        #region Grouping Properties

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("False")]
        private bool _collapseLastGroup = false;
        public bool CollapseLastGroup
        {
            get { return _collapseLastGroup; }
            set
            {
                if (_collapseLastGroup != value)
                {
                    _collapseLastGroup = value;
                    OnPropertyChanged("CollapseLastGroup");
                }
            }
        }

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


        #endregion Grouping Properties

        #region Freezing Properties


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


        #endregion Freezing Properties


        #region Filter Properties


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
        private bool _canUserFilter = false;
        public bool CanUserFilter
        {
            get { return _canUserFilter; }
            set
            {
                if (_canUserFilter != value)
                {
                    _canUserFilter = value;
                    OnPropertyChanged("CanUserFilter");
                }
            }
        }
        #endregion Filter Properties


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
                        AllSelectedPropertyChanged selectedModel = item as AllSelectedPropertyChanged;
                        CheckBox cb = (Mouse.DirectlyOver as UIElement) as CheckBox;
                        if (cb == null || cb.DataContext != selectedModel)
                        {
                            selectedModel.IsSelected = "1";
                        }

                    }
                }

            }

        }
        
        public bool AllChecked
        {
            get { return (bool)GetValue(AllCheckedProperty); }
            set { SetValue(AllCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllCheckedProperty =
            DependencyProperty.Register("AllChecked", typeof(bool), typeof(DataGridExtend), new PropertyMetadata(true));



        public event CheckedSelectedChangedEvent CheckedSelectedChanged;
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
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
                    int count = 0;
                    foreach (var item in FilteredItemsSource)
                    {
                        count++;
                    }
                    if (count > 0)
                    {
                        AllChecked = true;
                        foreach (var item in FilteredItemsSource)
                        {
                            AllSelectedPropertyChanged selectedModel = item as AllSelectedPropertyChanged;
                            if (selectedModel == null)
                                return;
                            selectedModel.PropertyChanged -= SelectedModel_PropertyChanged;
                            selectedModel.PropertyChanged += SelectedModel_PropertyChanged;
                            if ((selectedModel.IsSelected != "1") && AllChecked)
                            {
                                AllChecked = false;
                            }
                        }
                    }
                    else
                    {
                        AllChecked = false;
                    }

                }

                if (FilterFinshItemsSource != null && FilterFinshItemsSource.Count > 0)
                    FilterFinshItemsSource.Clear();
                if (btnFilter != null)
                    btnFilter.Content = "M9.4229997,18.347 C14.338999,18.347 18.345999,14.339 18.345999,9.4229999 18.345999,4.508 14.338999,0.5 9.4229997,0.5 4.5079998,0.5 0.5,4.508 0.5,9.4229999 0.5,14.339 4.5079998,18.347 9.4229997,18.347 z M15.733999,15.734 L17.732999,17.733 M16.972,16.972 L18.5,18.5";
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
            AllSelectedPropertyChanged selectedModel = sender as AllSelectedPropertyChanged;
            if (selectedModel.IsSelected == "1")
            {
                AllChecked = true;
                if (!IsDefaultSelectAll)
                {
                    foreach (var item in FilteredItemsSource)
                    {
                        AllSelectedPropertyChanged model = item as AllSelectedPropertyChanged;
                        if (model == null)
                            continue;
                        if (model.IsSelected == "0" || model.IsSelected == null || model.IsSelected == "")
                        {
                            AllChecked = false;
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
            //if (this.Filters != null)
            //    foreach (var filter in this.Filters)
            //        filter.ResetControl();
        }
        private void CbAllCheck_Click(object sender, RoutedEventArgs e)
        {
            IsDefaultSelectAll = true;
            foreach (var item in FilteredItemsSource)
            {
                AllSelectedPropertyChanged selectedModel = item as AllSelectedPropertyChanged;
                if (selectedModel == null)
                    continue;
                if (AllChecked)
                {
                    if (selectedModel.IsSelected != "1")
                        selectedModel.IsSelected = "1";
                }
                else
                {
                    if (selectedModel.IsSelected != "0")
                        selectedModel.IsSelected = "0";
                }
            }
            if (CheckedSelectedChanged != null)
            {
                CheckedSelectedChanged(null, new CheckSelectedEventArgs(true));
            }
            IsDefaultSelectAll = false;
            //if (this.Filters != null)
            //    foreach (var filter in this.Filters)
            //        filter.ResetControl();
        }
        #endregion

        private void BtnFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            this.CanUserFilter = false;
        }

        private void BtnFilter_Checked(object sender, RoutedEventArgs e)
        {
            this.CanUserFilter = true;
        }
        
        void filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilterChanged")
            {
                ColumnFilterControl cf = sender as ColumnFilterControl;

                Predicate<object> predicate = null;
                if (!IsFilterWindow)
                {
                    if (window != null)
                    {
                        predicate = FilterDataViewModel.Instance.FilterPredicate;
                        window.Close();
                    }
                }
                else
                {
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
                                predicate = predicate.And(filter.GeneratePredicate());
                        }
                    }
                }
                bool canContinue = true;
                var args = new CancelableFilterChangedEventArgs(predicate);
                if (BeforeFilterChanged != null && !IsResetting)
                {
                    BeforeFilterChanged(this, args);
                    canContinue = !args.Cancel;
                }
                if (canContinue)
                {
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
                        
                        int count = 1;
                        ObservableCollection<AllSelectedPropertyChanged> list = new ObservableCollection<AllSelectedPropertyChanged>();

                        foreach (var item in CollectionView)
                        {
                            AllSelectedPropertyChanged selectedModel = item as AllSelectedPropertyChanged;
                            //selectedModel.Num= count++;
                            list.Add(selectedModel);
                        }
                        foreach (var item in list)
                        {
                            if (item == null)
                                continue;
                            item.Num = count++;
                        }
                        //if(cf.FilterText!=null&& cf.FilterText.Trim() != "")
                        //{
                        if (list != null && list.Count > 0)
                        {
                            FilterFinshItemsSource = list;

                            if (AfterFilterChanged != null)
                                AfterFilterChanged(this, new FilterChangedEventArgs(predicate));
                        }
                                
                        //}
                        
                    }
                       
                    
                }
                else
                {
                    IsResetting = true;
                    var ctrl = sender as ColumnFilterControl;
                    ctrl.ResetControl();
                    IsResetting = false;
                }
            }
        }

        internal void RegisterOptionControl(ColumnFilterControl ctrl)
        {
            if (!Filters.Contains(ctrl))
            {
                ctrl.PropertyChanged += _filterHandler;
                Filters.Add(ctrl);
            }
        }


        #region Grouping

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

        public void ClearGroups()
        {
            if (CollectionView != null && CollectionView.GroupDescriptions != null)
                CollectionView.GroupDescriptions.Clear();
        }
        #endregion Grouping

        #region Freezing

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
        public void UnFreezeColumn(DataGridColumn column)
        {
            if (this.FrozenColumnCount > 0 && column.IsFrozen && this.Columns != null && this.Columns.Contains(column))
            {
                this.FrozenColumnCount--;
                column.DisplayIndex = this.FrozenColumnCount;
            }
        }

        public void UnFreezeAllColumns()
        {
            for (int i = Columns.Count - 1; i >= 0; i--)
                UnFreezeColumn(Columns[i]);
        }

        #endregion Freezing

        public void ShowFilter(DataGridColumn column, Visibility visibility)
        {
            var ctrl = Filters.Where(i => i.FilterColumnInfo.Column == column).FirstOrDefault();
            if (ctrl != null)
                ctrl.FilterVisibility = visibility;
        }

        public void ConfigureFilter(DataGridColumn column, bool canUserSelectDistinct, bool canUserGroup, bool canUserFreeze, bool canUserFilter)
        {
            column.SetValue(ColumnConfiguration.CanUserFilterProperty, canUserFilter);
            column.SetValue(ColumnConfiguration.CanUserFreezeProperty, canUserFreeze);
            column.SetValue(ColumnConfiguration.CanUserGroupProperty, canUserGroup);
            column.SetValue(ColumnConfiguration.CanUserSelectDistinctProperty, canUserSelectDistinct);

            var ctrl = Filters.Where(i => i.FilterColumnInfo.Column == column).FirstOrDefault();
            if (ctrl != null)
            {
                ctrl.CanUserSelectDistinct = canUserSelectDistinct;
                ctrl.CanUserGroup = canUserGroup;
                ctrl.CanUserFreeze = canUserFreeze;
                //ctrl.CanUserFilter = canUserFilter;
            }
        }

        public void ResetDistinctLists()
        {
            foreach (var optionControl in Filters)
                optionControl.ResetDistinctList();
        }

        internal void RegisterColumnOptionControl(ColumnOptionControl columnOptionControl)
        {
            _optionControls.Add(columnOptionControl);
        }
        internal void UpdateColumnOptionControl(ColumnFilterControl columnFilterControl)
        {
            //Since visibility for column contrls is set off the ColumnFilterControl by the base grid, we need to 
            //update the ColumnOptionControl since it is a seperate object.
            var ctrl = _optionControls.Where(c => c.FilterColumnInfo != null && columnFilterControl.FilterColumnInfo != null && c.FilterColumnInfo.Column == columnFilterControl.FilterColumnInfo.Column).FirstOrDefault();
            if (ctrl != null)
                ctrl.ResetVisibility();
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
