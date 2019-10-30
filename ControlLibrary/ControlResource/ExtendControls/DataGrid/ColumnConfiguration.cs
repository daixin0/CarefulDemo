using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
namespace ControlResource.ExtendControlStyle.DataGrid
{
    public class ColumnConfiguration : DependencyObject
    {
     
        #region Properties for setting via xaml

        public static readonly DependencyProperty CanUserFreezeProperty =
    DependencyProperty.RegisterAttached("CanUserFreeze", typeof(bool?), typeof(ColumnConfiguration), new PropertyMetadata(null));

        public static void SetCanUserFreeze(DependencyObject element, object o)
        {
            element.SetValue(CanUserFreezeProperty, o);
        }

        public static bool GetCanUserFreeze(DependencyObject element)
        {
            return (bool)element.GetValue(CanUserFreezeProperty);
        }

        public static readonly DependencyProperty CanUserFilterProperty =
DependencyProperty.RegisterAttached("CanUserFilter", typeof(bool?), typeof(ColumnConfiguration), new PropertyMetadata(null));

        public static void SetCanUserFilter(DependencyObject element, object o)
        {
            element.SetValue(CanUserFilterProperty, o);
        }

        public static bool GetCanUserFilter(DependencyObject element)
        {
            return (bool)element.GetValue(CanUserFilterProperty);
        }

        public static readonly DependencyProperty CanUserGroupProperty =
DependencyProperty.RegisterAttached("CanUserGroup", typeof(bool?), typeof(ColumnConfiguration), new PropertyMetadata(null));

        public static void SetCanUserGroup(DependencyObject element, object o)
        {
            element.SetValue(CanUserGroupProperty, o);
        }

        public static bool GetCanUserGroup(DependencyObject element)
        {
            return (bool)element.GetValue(CanUserGroupProperty);
        }

        public static readonly DependencyProperty CanUserSelectDistinctProperty =
DependencyProperty.RegisterAttached("CanUserSelectDistinct", typeof(bool?), typeof(ColumnConfiguration), new PropertyMetadata(null));

        public static void SetCanUserSelectDistinct(DependencyObject element, object o)
        {
            element.SetValue(CanUserSelectDistinctProperty, o);
        }

        public static bool GetCanUserSelectDistinct(DependencyObject element)
        {
            return (bool)element.GetValue(CanUserSelectDistinctProperty);
        }

        public static readonly DependencyProperty DefaultFilterProperty =
DependencyProperty.RegisterAttached("DefaultFilter", typeof(string), typeof(ColumnConfiguration), new PropertyMetadata(null));

        public static void SetDefaultFilter(DependencyObject element, object o)
        {
            element.SetValue(DefaultFilterProperty, o);
        }

        public static string GetDefaultFilter(DependencyObject element)
        {
            return (string)element.GetValue(DefaultFilterProperty);
        }

        #endregion
    }
}
