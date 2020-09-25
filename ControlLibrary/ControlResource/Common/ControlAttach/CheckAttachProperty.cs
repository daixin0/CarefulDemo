using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Careful.Controls.Common.AttachProperty
{
    public class CheckAttachProperty: DependencyObject
    {
        public static PlacementMode GetContextPlacementMode(DependencyObject obj)
        {
            return (PlacementMode)obj.GetValue(ContextPlacementModeProperty);
        }

        public static void SetContextPlacementMode(DependencyObject obj, PlacementMode value)
        {
            obj.SetValue(ContextPlacementModeProperty, value);
        }

        // Using a DependencyProperty as the backing store for ContextPlacementMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContextPlacementModeProperty =
            DependencyProperty.RegisterAttached("ContextPlacementMode", typeof(PlacementMode), typeof(CheckAttachProperty), new PropertyMetadata(PlacementMode.Bottom));

        public static readonly DependencyProperty ShowContextMenuByLeftClickProperty =
      DependencyProperty.RegisterAttached("ShowContextMenuByLeftClick", typeof(bool), typeof(CheckAttachProperty), new PropertyMetadata(false, OnShowContextMenuByLeftClickChanged));

        public static bool GetShowContextMenuByLeftClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowContextMenuByLeftClickProperty);
        }

        public static void SetShowContextMenuByLeftClick(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowContextMenuByLeftClickProperty, value);
        }
        private static void OnShowContextMenuByLeftClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = d as ToggleButton;

            if (btn == null)
            {
                throw new Exception("Cannot assign this attached property to a control other than Button");
            }

            if ((bool)e.NewValue)
            {
                // 设置为 true
                btn.Click -= Btn_Click;
                btn.Click += Btn_Click; ;

                btn.MouseRightButtonUp -= Btn_MouseRightButtonUp;
                btn.MouseRightButtonUp += Btn_MouseRightButtonUp;
            }
            else
            {
                btn.Click -= Btn_Click;
                btn.MouseRightButtonUp -= Btn_MouseRightButtonUp;
            }
        }

        private static void Btn_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private static void Btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as ToggleButton;

            if (btn.ContextMenu == null)
            {
                throw new Exception("The button does not has a ContextMenu");
            }

            if (btn.IsChecked == true)
            {
                btn.ContextMenu.Placement = GetContextPlacementMode(btn);
                btn.ContextMenu.PlacementTarget = btn;
                //btn.ContextMenu.HorizontalOffset = 310;
                btn.ContextMenu.IsOpen = true;
            }
           
        }
    }
}
