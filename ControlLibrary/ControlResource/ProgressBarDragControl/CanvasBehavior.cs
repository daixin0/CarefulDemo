using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Careful.Controls.ProgressBarDragControl
{
    public class CanvasBehavior : Behavior<UIElement>
    {

        public ProgressBarDrag ProgressControl
        {
            get { return (ProgressBarDrag)GetValue(ProgressControlProperty); }
            set { SetValue(ProgressControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressControlProperty =
            DependencyProperty.Register("ProgressControl", typeof(ProgressBarDrag), typeof(CanvasBehavior));



        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
        }


        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mouseOffset = e.GetPosition(AssociatedObject);

            double radio = (ProgressControl.Maximum - ProgressControl.Minimum) / (AssociatedObject as Canvas).ActualWidth;
            //ProgressControl.Value = radio * mouseOffset.X + ProgressControl.Minimum;
            double progress = radio * mouseOffset.X;
            if (ProgressControl.Value < ProgressControl.Minimum || ProgressControl.Value > ProgressControl.Maximum)
            {
                return;
            }
            ProgressControl.Value = progress;
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(ProgressBarDrag.PressedPositionEvent, null);
            ProgressControl.RaiseEvent(routedEventArgs);

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
        }
    }
}
