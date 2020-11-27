using Careful.Controls.Common;
using Careful.Core.Mvvm.Command;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Careful.Controls.ListBoxExtendControl
{
    public delegate void CheckedSelectedChangedEvent(object sender, CheckSelectedEventArgs e);
    public class ListBoxExtend : ListBox
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (IsGroup)
            {
                return item is ListBoxItemExtend;
            }
            else
            {
                return item is ListBoxItem;
            }

        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            if (IsGroup)
            {
                ListBoxItemExtend item = new ListBoxItemExtend();
                return item;
            }
            else
            {
                ListBoxItem item = new ListBoxItem();
                return item;
            }
        }



        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGroup.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(ListBoxExtend));



        #region Check All


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CheckBox checkBox = this.Template.FindName("cbAll", this) as CheckBox;
            checkBox.Click -= CheckBox_Click;
            checkBox.Click += CheckBox_Click;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            AllCheckClick();
        }

        public bool? AllChecked
        {
            get { return (bool?)GetValue(AllCheckedProperty); }
            set { SetValue(AllCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllCheckedProperty =
            DependencyProperty.Register("AllChecked", typeof(bool?), typeof(ListBoxExtend), new PropertyMetadata(true));



        private void AllCheckClick()
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

        public int CheckNumber
        {
            get { return (int)GetValue(CheckNumberProperty); }
            set { SetValue(CheckNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckNumberProperty =
            DependencyProperty.Register("CheckNumber", typeof(int), typeof(ListBoxExtend));




        public event CheckedSelectedChangedEvent CheckedSelectedChanged;

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this.Items.Count > 0)
            {
                if (IsGroup)
                {
                    foreach (var item in ItemsSource)
                    {
                        if (item is Group group)
                        {
                            bool isExistSelect = false;
                            bool isExistNoSelect = false;
                            group.GroupCheckCommand = new RelayCommand(GroupCheck);
                            if (group.Children != null)
                            {
                                foreach (var child in group.Children)
                                {
                                    if (child is IGroupItem selectedModel)
                                    {
                                        if (selectedModel == null)
                                            return;
                                        selectedModel.ParentGroup = group;
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
                                    }
                                }
                            }
                            if (isExistSelect && !isExistNoSelect)
                            {
                                group.IsSelected = true;
                            }
                            else if (!isExistSelect && isExistNoSelect)
                            {
                                group.IsSelected = false;
                            }
                            else if (isExistSelect && isExistNoSelect)
                            {
                                group.IsSelected = null;
                            }
                        }

                    }

                }
                else
                {
                    bool isExistSelect = false;
                    bool isExistNoSelect = false;
                    foreach (var item in ItemsSource)
                    {
                        if (item is ISelected selectedModel)
                        {
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
                        }
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

            }
            else
            {
                AllChecked = false;
            }
        }
        private void GroupCheck(object obj)
        {
            if (obj is Group group)
            {
                IsDefaultSelectAll = true;
                if (group.Children != null)
                {
                    foreach (var item in group.Children)
                    {
                        ISelected selectedModel = item as ISelected;
                        if (selectedModel == null)
                            continue;
                        if (group.IsSelected == true)
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
                }

                IsDefaultSelectAll = false;
            }
        }

        public bool IsDefaultSelectAll
        {
            get { return (bool)GetValue(IsDefaultSelectAllProperty); }
            set { SetValue(IsDefaultSelectAllProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDefaultSelectAll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDefaultSelectAllProperty =
            DependencyProperty.Register("IsDefaultSelectAll", typeof(bool), typeof(ListBoxExtend));


        private void SelectedModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsSelected")
                return;
            if (IsGroup)
            {
                IGroupItem selectedModel = sender as IGroupItem;
                if (selectedModel.IsSelected)
                {
                    selectedModel.ParentGroup.IsSelected = true;
                    if (!IsDefaultSelectAll)
                    {
                        foreach (var item in selectedModel.ParentGroup.Children)
                        {
                            IGroupItem model = item as IGroupItem;
                            if (model == null)
                                continue;
                            if (!model.IsSelected)
                            {
                                model.ParentGroup.IsSelected = null;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    selectedModel.ParentGroup.IsSelected = false;
                    foreach (var item in selectedModel.ParentGroup.Children)
                    {
                        IGroupItem model = item as IGroupItem;
                        if (model == null)
                            continue;
                        if (model.IsSelected)
                        {
                            model.ParentGroup.IsSelected = null;
                            break;
                        }
                    }
                }
            }
            else
            {
                CheckNumber = 0;
                foreach (var item in this.Items)
                {
                    if (item is ISelected selected && selected.IsSelected)
                    {
                        CheckNumber++;
                    }
                }
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
                }
            }
          
        }
        #endregion
    }
    public class ListBoxItemExtend : ListBoxItem
    {

    }
}
