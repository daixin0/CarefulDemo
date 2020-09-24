using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Careful.Controls.TreeListViewControl
{
    public class TreeListView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem item = new TreeListViewItem();
            item.GridViewColumns = GridViewColumns;
            return item;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            //return item is TreeListViewItem;
            bool _isTreeLVI = item is TreeListViewItem;
            return _isTreeLVI;
        }

        public GridViewColumnCollection GridViewColumns
        {
            get { return (GridViewColumnCollection)GetValue(GridViewColumnsProperty); }
            set { SetValue(GridViewColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridViewColumns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridViewColumnsProperty =
            DependencyProperty.Register("GridViewColumns", typeof(GridViewColumnCollection), typeof(TreeListView));



        private static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while ((source != null) && (source.GetType() != typeof(T)))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }

        private TreeListViewItem FindTreeViewItem(ItemsControl item, object data)
        {
            TreeListViewItem findItem = null;
            bool itemIsExpand = false;
            if (item is TreeListViewItem)
            {
                TreeListViewItem tviCurrent = item as TreeListViewItem;
                object itemData = tviCurrent.DataContext;
                if (itemData == data)
                {
                    findItem = tviCurrent;
                }
                else
                {
                    itemIsExpand = tviCurrent.IsExpanded;
                    if (!tviCurrent.IsExpanded)
                    {
                        tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, true);
                        tviCurrent.UpdateLayout();
                    }
                }
            }
            if (findItem == null)
            {
                for (int i = 0; i < item.Items.Count; i++)
                {
                    TreeListViewItem tvItem = (TreeListViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
                    if (tvItem == null)
                        continue;
                    object itemData = item.Items[i];
                    if (itemData == data)
                    {
                        findItem = tvItem;
                        break;
                    }
                    else if (tvItem.Items.Count > 0)
                    {
                        findItem = FindTreeViewItem(tvItem, data);
                        if (findItem != null)
                            break;
                    }
                }
                TreeListViewItem tviCurrent = item as TreeListViewItem;
                tviCurrent.SetValue(TreeListViewItem.IsExpandedProperty, itemIsExpand);
                tviCurrent.UpdateLayout();

            }
            return findItem;

        }
        int count = 0;
        private bool ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect)
        {
            foreach (Object item in parentContainer.Items)
            {
                count++;
                TreeListViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeListViewItem;
                if (item == itemToSelect && currentContainer != null)
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();
                    ScrollViewer viewer = VisualUpwardSearch<ScrollViewer>(currentContainer as DependencyObject) as ScrollViewer;
                    viewer.ScrollToVerticalOffset(count * 25);
                    return true;
                }

                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    bool wasExpanded = currentContainer.IsExpanded;
                    currentContainer.IsExpanded = true;
                    currentContainer.UpdateLayout();
                    if (ExpandAndSelectItem(currentContainer, itemToSelect))
                    {
                        return true;
                    }
                    else
                    {
                        currentContainer.IsExpanded = wasExpanded;
                        currentContainer.UpdateLayout();
                    }
                }

                
            }
            return false;
        }

        public void SetScorllViewPosition(object item, object oneLevelTree = null)
        {
            if (item == null)
                return;
            TreeListViewItem container = null;
            if (oneLevelTree == null)
            {
                foreach (var tree in this.Items)
                {
                    TreeListViewItem tlvi = this.ItemContainerGenerator.ContainerFromItem(tree) as TreeListViewItem;
                    container = FindTreeViewItem(tlvi, item) as TreeListViewItem;
                    if (container != null)
                        break;

                }
                if (container == null)
                    return;
                container.BringIntoView();
                container.IsSelected = true;
            }
            else
            {

                TreeListViewItem tlvi = this.ItemContainerGenerator.ContainerFromItem(oneLevelTree) as TreeListViewItem;
                if (item == oneLevelTree)
                {
                    tlvi.IsSelected = true;
                    tlvi.BringIntoView();
                    tlvi.Focus();
                }
                else
                {
                    tlvi.IsExpanded = true;
                    tlvi.UpdateLayout();
                    count = 0;
                    foreach (Object itemTree in this.Items)
                    {
                        count++;
                        if (itemTree == oneLevelTree)
                        {
                            break;
                        }
                    }
                    ExpandAndSelectItem(tlvi, item);
                }
            }
            
        }

    }


    public class TreeListViewItem : TreeViewItem
    {
        /// <summary>
        /// hierarchy 
        /// </summary>
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    TreeListViewItem parent =
                        ItemsControl.ItemsControlFromItemContainer(this) as TreeListViewItem;//返回拥有指定的容器元素中 ItemsControl 。 
                    _level = (parent != null) ? parent.Level + 1 : 0;
                }
                return _level;
            }
        }

        private static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while ((source != null) && (source.GetType() != typeof(T)))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            TreeListViewItem tvitem = VisualUpwardSearch<TreeListViewItem>(e.OriginalSource as DependencyObject) as TreeListViewItem;
            if (tvitem != null)
            {
                tvitem.Focus();
            }

            base.OnPreviewMouseRightButtonDown(e);

        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeListViewItem item = new TreeListViewItem();
            item.GridViewColumns = GridViewColumns;
            return item;
        }

        public GridViewColumnCollection GridViewColumns
        {
            get { return (GridViewColumnCollection)GetValue(GridViewColumnsProperty); }
            set { SetValue(GridViewColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridViewColumns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridViewColumnsProperty =
            DependencyProperty.Register("GridViewColumns", typeof(GridViewColumnCollection), typeof(TreeListViewItem));


        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            //return item is TreeListViewItem;
            bool _isITV = item is TreeListViewItem;
            return _isITV;
        }

        protected override void OnCollapsed(RoutedEventArgs e)
        {
            base.OnCollapsed(e);
            try
            {
                TreeListViewItem t = e.Source as TreeListViewItem;
                if (t == null)
                    return;
                ControlTemplate template = t.Template;

                Storyboard hide = template.Resources["STHide"] as Storyboard;
                ItemsPresenter item = template.FindName("ItemsHost", t) as ItemsPresenter;
                hide.Begin(item);
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
            }
        }
        protected override void OnExpanded(RoutedEventArgs e)
        {
            base.OnExpanded(e);
            try
            {
                TreeListViewItem t = e.Source as TreeListViewItem;
                if (t == null)
                    return;
                ControlTemplate template = t.Template;
                Storyboard show = template.Resources["STShow"] as Storyboard;
                ItemsPresenter item = template.FindName("ItemsHost", t) as ItemsPresenter;
                show.Begin(item);
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private int _level = -1;
    }
}
