using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ControlResource.ExtendControlStyle.TreeListView
{
    public class TreeViewExtensions : DependencyObject
    {
        public static bool GetEnableMultiSelect(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableMultiSelectProperty);
        }
        
        public static void SetEnableMultiSelect(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableMultiSelectProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableMultiSelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableMultiSelectProperty =
            DependencyProperty.RegisterAttached("EnableMultiSelect", typeof(bool), typeof(TreeViewExtensions), new FrameworkPropertyMetadata(false)
            {
                PropertyChangedCallback = EnableMultiSelectChanged,
                BindsTwoWayByDefault = true
            });
        
        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }
        
        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(TreeViewExtensions),new PropertyMetadata(null));
        
        static TreeListViewItem GetAnchorItem(DependencyObject obj)
        {
            return (TreeListViewItem)obj.GetValue(AnchorItemProperty);
        }
        
        static void SetAnchorItem(DependencyObject obj, TreeViewItem value)
        {
            obj.SetValue(AnchorItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnchorItem.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty AnchorItemProperty =
            DependencyProperty.RegisterAttached("AnchorItem", typeof(TreeViewItem), typeof(TreeViewExtensions), new PropertyMetadata(null));
        
        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }
        private static List<object> _selectedTreeItems;
        public static List<object> SelectedTreeItems
        {
            get { return _selectedTreeItems; }
            set { _selectedTreeItems = value; }
        }
        private static bool _isKeyCtrl;
        public static bool IsKeyCtrl
        {
            get { return _isKeyCtrl; }
            set { _isKeyCtrl = value; }
        }
        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            TreeListViewItem tlvi = obj as TreeListViewItem;
            //if (IsKeyCtrl && tlvi.IsSelected)
            //{
            //    return;
            //}
            if (value)
            {
                GradientStopCollection gradientStopCollection = new GradientStopCollection();
                gradientStopCollection.Add(new GradientStop()
                {
                    Color = (Color)ColorConverter.ConvertFromString("#FFC7DFFC"),
                    Offset = 1
                });
                gradientStopCollection.Add(new GradientStop()
                {
                    Color = (Color)ColorConverter.ConvertFromString("#FF3832B8"),
                    Offset = 1
                });
                LinearGradientBrush brush = new LinearGradientBrush(gradientStopCollection, new Point(0.5, 0), new Point(0.5, 1));
                tlvi.Background = brush;
                if (SelectedTreeItems == null)
                    SelectedTreeItems = new List<object>();
                if (!SelectedTreeItems.Contains(tlvi.DataContext))
                {
                    SelectedTreeItems.Add(tlvi.DataContext);
                }
            }
            else
            {
                tlvi.Background = Brushes.Transparent;
                if (SelectedTreeItems != null)
                {
                    if (SelectedTreeItems.Contains(tlvi.DataContext))
                    {
                        SelectedTreeItems.Remove(tlvi.DataContext);
                    }
                }
                    
            }
            obj.SetValue(IsSelectedProperty, value);
            
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(TreeViewExtensions), new PropertyMetadata(false)
            {
                PropertyChangedCallback = RealSelectedChanged
            });
        
        static void EnableMultiSelectChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
        {

            TreeListView tree = (TreeListView)s;
            var wasEnable = (bool)args.OldValue;
            var isEnabled = (bool)args.NewValue;
            if (wasEnable)
            {
                tree.RemoveHandler(TreeViewItem.MouseDownEvent, new MouseButtonEventHandler(ItemClicked));
                tree.RemoveHandler(TreeViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(MouseDoubleClick));
                tree.RemoveHandler(TreeView.KeyDownEvent, new KeyEventHandler(KeyDown));
                tree.RemoveHandler(TreeView.KeyUpEvent, new KeyEventHandler(KeyUp));
            }
            if (isEnabled)
            {
                SelectedTreeItems = new List<object>();
                tree.AddHandler(TreeViewItem.MouseDownEvent, new MouseButtonEventHandler(ItemClicked), true);
                tree.AddHandler(TreeViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(MouseDoubleClick), true);
                tree.AddHandler(TreeView.KeyDownEvent, new KeyEventHandler(KeyDown));
                tree.AddHandler(TreeView.KeyUpEvent, new KeyEventHandler(KeyUp));
            }
        }
        
        
        static TreeListView GetTree(TreeListViewItem item)
        {
            Func<DependencyObject, DependencyObject> getParent = (o) => VisualTreeHelper.GetParent(o);
            FrameworkElement currentItem = item;
            while (!(getParent(currentItem) is TreeListView))
            {
                currentItem = (FrameworkElement)getParent(currentItem);
            }
            return (TreeListView)getParent(currentItem);
        }
        
        static void RealSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TreeListViewItem item = (TreeListViewItem)sender;
            var selectedItems = GetSelectedItems(GetTree(item));
            if (selectedItems != null)
            {
                var isSelected = GetIsSelected(item);
                if (isSelected)
                {
                    try
                    {
                        selectedItems.Add(item.Header);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                else
                {
                    selectedItems.Remove(item.Header);
                }
            }
        }
        
        static void KeyDown(object sender, KeyEventArgs e)
        {
            TreeListView tree = (TreeListView)sender;
            if (e.Key == Key.A && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                foreach (var item in GetExpandedTreeViewItems(tree))
                {
                    SetIsSelected(item, true);
                }
                e.Handled = true;
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                IsKeyCtrl = true;
                e.Handled = true;
            }
        }
        static void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl||e.Key==Key.RightCtrl)
            {
                IsKeyCtrl = false;
            }
        }
        static void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeListView tree = (TreeListView)sender;
            foreach (var item in GetExpandedTreeViewItems(tree))
            {
                SetIsSelected(item, false);
            }
        }
        static void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            TreeListViewItem item = FindTreeViewItem(e.OriginalSource);
            if (item == null)
            {
                return;
            }
            TreeListView tree = (TreeListView)sender;

            var mouseButton = e.ChangedButton;
            if (mouseButton != MouseButton.Left)
            {
                if ((mouseButton == MouseButton.Right) && ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == ModifierKeys.None))
                {
                    if (GetIsSelected(item))
                    {
                        UpdateAnchorAndActionItem(tree, item);
                        return;
                    }
                    MakeSingleSelection(tree, item);
                }
                return;
            }
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != (ModifierKeys.Shift | ModifierKeys.Control))
            {
                if (GetIsSelected(item)&& Keyboard.Modifiers != ModifierKeys.Shift && Keyboard.Modifiers != ModifierKeys.Control)
                {
                    return;
                }
                else if(Keyboard.Modifiers != ModifierKeys.Shift && Keyboard.Modifiers != ModifierKeys.Control)
                {
                    SelectedTreeItems.Clear();
                }
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    MakeToggleSelection(tree, item);
                    return;
                }
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    MakeAnchorSelection(tree, item, true);
                    return;
                }
                
                MakeSingleSelection(tree, item);
                return;
            }
        }
        
        private static TreeListViewItem FindTreeViewItem(object obj)
        {
            DependencyObject dpObj = obj as DependencyObject;
            if (dpObj == null)
            {
                return null;
            }
            if (dpObj is TreeListViewItem)
            {
                return (TreeListViewItem)dpObj;
            }
            return FindTreeViewItem(VisualTreeHelper.GetParent(dpObj));
        }
        
        private static IEnumerable<TreeListViewItem> GetExpandedTreeViewItems(ItemsControl tree)
        {
            for (int i = 0; i < tree.Items.Count; i++)
            {
                var item = (TreeListViewItem)tree.ItemContainerGenerator.ContainerFromIndex(i);
                if (item == null)
                {
                    continue;
                }
                yield return item;
                if (item.IsExpanded)
                {
                    foreach (var subItem in GetExpandedTreeViewItems(item))
                    {
                        yield return subItem;
                    }
                }
            }
        }
        
        private static void MakeAnchorSelection(TreeListView tree, TreeListViewItem actionItem, bool clearCurrent)
        {
            if (GetAnchorItem(tree) == null)
            {
                var selectedItems = GetSelectedTreeViewItems(tree);
                if (selectedItems.Count > 0)
                {
                    SetAnchorItem(tree, selectedItems[selectedItems.Count - 1]);
                }
                else
                {
                    SetAnchorItem(tree, GetExpandedTreeViewItems(tree).Skip(3).FirstOrDefault());
                }
                if (GetAnchorItem(tree) == null)
                {
                    return;
                }
            }

            var anchor = GetAnchorItem(tree);

            var items = GetExpandedTreeViewItems(tree);
            bool betweenBoundary = false;
            foreach (var item in items)
            {
                bool isBoundary = item == anchor || item == actionItem;
                if (isBoundary)
                {
                    betweenBoundary = !betweenBoundary;
                }
                if (betweenBoundary || isBoundary)
                {
                    SetIsSelected(item, true);
                }
                else
                {
                    if (clearCurrent)
                    {
                        SetIsSelected(item, false);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        public static List<TreeListViewItem> GetSelectedTreeViewItems(TreeListView tree)
        {
            return GetExpandedTreeViewItems(tree).Where(i => GetIsSelected(i)).ToList();
        }
        
        private static void MakeSingleSelection(TreeListView tree, TreeListViewItem item)
        {
            foreach (TreeListViewItem selectedItem in GetExpandedTreeViewItems(tree))
            {
                if (selectedItem == null)
                {
                    continue;
                }
                if (selectedItem != item)
                {
                    SetIsSelected(selectedItem, false);
                }
                else
                {
                    SetIsSelected(selectedItem, true);
                }
            }
            UpdateAnchorAndActionItem(tree, item);
        }
        
        private static void MakeToggleSelection(TreeListView tree, TreeListViewItem item)
        {
            SetIsSelected(item, !GetIsSelected(item));
            UpdateAnchorAndActionItem(tree, item);
        }
        
        private static void UpdateAnchorAndActionItem(TreeListView tree, TreeListViewItem item)
        {
            SetAnchorItem(tree, item);
        }
    }
}
