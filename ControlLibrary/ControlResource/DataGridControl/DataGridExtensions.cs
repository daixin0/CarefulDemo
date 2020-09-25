using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Careful.Controls.DataGridControl
{
    public class DataGridExtensions : DependencyObject
    {


        public static DataGridExtend GetCurrentGrid(DependencyObject obj)
        {
            return (DataGridExtend)obj.GetValue(CurrentGridProperty);
        }

        public static void SetCurrentGrid(DependencyObject obj, DataGridExtend value)
        {
            obj.SetValue(CurrentGridProperty, value);
        }

        // Using a DependencyProperty as the backing store for CurrentGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentGridProperty =
            DependencyProperty.RegisterAttached("CurrentGrid", typeof(DataGridExtend), typeof(DataGridExtensions));


        public static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    visualCollection.Add(child as T);
                }
                else if (child != null)
                {
                    GetVisualChildCollection(child, visualCollection);
                }
            }
        }
        public static DataGridColumnHeader GetCurrentSortColumn(DependencyObject obj)
        {
            return (DataGridColumnHeader)obj.GetValue(CurrentSortColumnProperty);
        }

        public static void SetCurrentSortColumn(DependencyObject obj, DataGridColumnHeader value)
        {
            obj.SetValue(CurrentSortColumnProperty, value);
        }

        // Using a DependencyProperty as the backing store for CurrentSortColumn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSortColumnProperty =
            DependencyProperty.RegisterAttached("CurrentSortColumn", typeof(DataGridColumnHeader), typeof(DataGridExtensions));
        

        public static bool? GetIsPositiveSequence(DependencyObject obj)
        {
            return (bool?)obj.GetValue(IsPositiveSequenceProperty);
        }

        public static void SetIsPositiveSequence(DependencyObject obj, bool? value)
        {
            obj.SetValue(IsPositiveSequenceProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsPositiveSequence.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPositiveSequenceProperty =
            DependencyProperty.RegisterAttached("IsPositiveSequence", typeof(bool?), typeof(DataGridExtensions), new PropertyMetadata(null, OnIsPositiveSequenceChanged));

        private static void OnIsPositiveSequenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridExtend dg = GetCurrentGrid(d);
            List<DataGridColumnHeader> columnHeaders = GetVisualChildCollection<DataGridColumnHeader>(dg);
            foreach (var item in columnHeaders)
            {
                ControlTemplate ct = item.Template;
                Path pAscending = ct.FindName("pAscending", item) as Path;
                Path pDescending = ct.FindName("pDescending", item) as Path;
                pAscending.Visibility = Visibility.Collapsed;
                pDescending.Visibility = Visibility.Collapsed;
            }

            DataGridColumnHeader colHeader = GetCurrentSortColumn(d) as DataGridColumnHeader;
            ControlTemplate template = colHeader.Template;
            if ((bool)e.NewValue == true)
            {
                Path pAscending = template.FindName("pAscending", colHeader) as Path;
                pAscending.Visibility = Visibility.Visible;
            }
            else if ((bool)e.NewValue == false)
            {
                Path pDescending = template.FindName("pDescending", colHeader) as Path;
                pDescending.Visibility = Visibility.Visible;
            }

        }
    }
}
