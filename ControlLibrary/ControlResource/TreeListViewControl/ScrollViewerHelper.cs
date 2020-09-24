using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Controls.TreeListViewControl
{
    public class ScrollViewerHelper
    {


        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(0d));
        
        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            SetHorizontalOffset(sender as DependencyObject, e.HorizontalOffset);
        }
        
        public static bool GetIsMonitor(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitorProperty);
        }

        public static void SetIsMonitor(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitorProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsMonitor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMonitorProperty =
            DependencyProperty.RegisterAttached("IsMonitor", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false, OnIsMonitorChanged));

        private static void OnIsMonitorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if ((bool)e.NewValue)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
            else
            {
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
        }

        public static double GetHorOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorOffsetProperty);
        }

        public static void SetHorOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorOffsetProperty, value);
        }

        // Using a DependencyProperty as the backing store for HorOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorOffsetProperty =
            DependencyProperty.RegisterAttached("HorOffset", typeof(double), typeof(ScrollViewerHelper), new PropertyMetadata(0d, OnHorOffSetChanged));

        private static void OnHorOffSetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }
    }


}
