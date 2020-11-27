using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Careful.Controls.ProgressBarDragControl
{
    public class ProgressDragBehavior : Behavior<UIElement>
    {
        private Canvas canvas;


        public ProgressBarDrag ProgressControl
        {
            get { return (ProgressBarDrag)GetValue(ProgressControlProperty); }
            set { SetValue(ProgressControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressControlProperty =
            DependencyProperty.Register("ProgressControl", typeof(ProgressBarDrag), typeof(ProgressDragBehavior));



        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(ProgressBarDrag.DragStopEvent, null);
            ProgressControl.RaiseEvent(routedEventArgs);
            AssociatedObject.ReleaseMouseCapture();
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (canvas == null)
                    return;
                Point point = e.GetPosition(canvas);
                if (point.X < 0 || point.X > canvas.ActualWidth)
                    return;
                //AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);

                double radio = (ProgressControl.Maximum - ProgressControl.Minimum) / canvas.ActualWidth;
                double progress = radio * point.X;
                if (ProgressControl.Value < ProgressControl.Minimum|| ProgressControl.Value> ProgressControl.Maximum)
                {
                    return;
                }
                ProgressControl.Value = progress;
                RoutedEventArgs routedEventArgs = new RoutedEventArgs(ProgressBarDrag.DragProcessEvent, null);
                ProgressControl.RaiseEvent(routedEventArgs);
            }
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.canvas == null)
            {
                canvas = VisualTreeHelper.GetParent(this.AssociatedObject) as Canvas;
            }
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(ProgressBarDrag.DragStartEvent, null);
            ProgressControl.RaiseEvent(routedEventArgs);

            AssociatedObject.CaptureMouse();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }
    }
}
