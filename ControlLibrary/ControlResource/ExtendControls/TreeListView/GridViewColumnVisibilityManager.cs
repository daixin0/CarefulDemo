using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ControlResource.ExtendControlStyle.TreeListView
{
    public class GridViewColumnVisibilityManager : GridViewColumn
    {

        static void UpdateListView(GridViewColumnCollection lv)
        {
            GridViewColumnCollection gridview = lv;
            if (gridview == null || gridview.Count<=0) return;
            List<GridViewColumn> toRemove = new List<GridViewColumn>();
            foreach (GridViewColumn gc in gridview)
            {
                if (GetIsVisible(gc) == false)
                {
                    toRemove.Add(gc);
                }
            }
            foreach (GridViewColumn gc in toRemove)
            {
                gridview.Remove(gc);
            }
        }

        public static bool GetIsVisible(DependencyObject obj)
        {
            bool b=(bool)obj.GetValue(IsVisibleProperty);
            return b;
        }
	 public static void SetIsVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsVisibleProperty, value);
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(GridViewColumnVisibilityManager), new UIPropertyMetadata(true));
        

        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(GridViewColumnVisibilityManager), new UIPropertyMetadata(false,
                new PropertyChangedCallback(OnEnabledChanged)));
        static UserControl view;

	private static void OnEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            view = obj as UserControl;
            if (view != null)
            {
                bool enabled = (bool)e.NewValue;
                if (enabled)
                {
                    ResourceDictionary rd = view.Resources;
                    GridViewColumnCollection gcc = rd["gvcc"] as GridViewColumnCollection;
                    view.Loaded += (sender, e2) =>
                    {
                        UpdateListView((GridViewColumnCollection)gcc);
                    };
                    view.TargetUpdated += (sender, e2) =>
                    {
                        UpdateListView((GridViewColumnCollection)gcc);
                    };
                    view.DataContextChanged += (sender, e2) =>
                    {
                        UpdateListView((GridViewColumnCollection)gcc);
                    };
                }
            }
        }
    }
}