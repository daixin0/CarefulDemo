using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Careful.Controls.TabControl3D
{
    public class AnimatedTabControl : TabControl
    {
        public static readonly RoutedEvent SelectionChangingEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanging", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AnimatedTabControl));

        private DispatcherTimer timer;

        public AnimatedTabControl()
        {
            DefaultStyleKey = typeof(AnimatedTabControl);
        }

        public event RoutedEventHandler SelectionChanging
        {
            add { AddHandler(SelectionChangingEvent, value); }
            remove { RemoveHandler(SelectionChangingEvent, value); }
        }
        int count = 0;
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                (Action)delegate
                {
                    if (count == 0)
                    {
                        count++;
                        return;
                    }
                    this.RaiseSelectionChangingEvent();

                    this.StopTimer();

                    this.timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500) };

                    EventHandler handler = null;
                    handler = (sender, args) =>
                        {
                            this.StopTimer();
                            base.OnSelectionChanged(e);
                        };
                    this.timer.Tick += handler;
                    this.timer.Start();
                });
        }

        private void RaiseSelectionChangingEvent()
        {
            var args = new RoutedEventArgs(SelectionChangingEvent);
            RaiseEvent(args);
        }

        private void StopTimer()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer = null;
            }
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            AnimatedTabControlItem item = new AnimatedTabControlItem();
            return item;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            //return item is TreeListViewItem;
            bool _isTreeLVI = item is AnimatedTabControlItem;
            return _isTreeLVI;
        }

    }

    public class AnimatedTabControlItem : TabItem
    {
    }
}