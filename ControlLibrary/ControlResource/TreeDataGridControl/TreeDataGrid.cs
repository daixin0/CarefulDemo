using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Controls.TreeDataGridControl
{
    public class TreeDataGrid : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeDataGridItem item = new TreeDataGridItem();
            return item;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool _isTreeLVI = item is TreeDataGridItem;
            return _isTreeLVI;
        }

    }
    public class TreeDataGridItem : TreeViewItem
    {
        private int _level = -1;
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    TreeDataGridItem parent =
                        ItemsControl.ItemsControlFromItemContainer(this) as TreeDataGridItem;
                    _level = (parent != null) ? parent.Level + 1 : 0;
                }
                return _level;
            }
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            TreeDataGridItem item = new TreeDataGridItem();
            return item;
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool _isITV = item is TreeDataGridItem;
            return _isITV;
        }


        public GridLength LevelWidth
        {
            get { return (GridLength)GetValue(LevelWidthProperty); }
            set { SetValue(LevelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LevelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LevelWidthProperty =
            DependencyProperty.Register("LevelWidth", typeof(GridLength), typeof(TreeDataGridItem),new PropertyMetadata(new GridLength(60)));



    }
}
