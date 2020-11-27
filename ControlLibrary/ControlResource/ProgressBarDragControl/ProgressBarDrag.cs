using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Controls.ProgressBarDragControl
{
    public class ProgressBarDrag : ProgressBar
    {



        public double ProgressHeight
        {
            get { return (double)GetValue(ProgressHeightProperty); }
            set { SetValue(ProgressHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressHeightProperty =
            DependencyProperty.Register("ProgressHeight", typeof(double), typeof(ProgressBarDrag));



        public CornerRadius ProgressCornerRadius
        {
            get { return (CornerRadius)GetValue(ProgressCornerRadiusProperty); }
            set { SetValue(ProgressCornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressCornerRadiusProperty =
            DependencyProperty.Register("ProgressCornerRadius", typeof(CornerRadius), typeof(ProgressBarDrag));




        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler DragStop
        {
            add { AddHandler(DragStopEvent, value); }
            remove { RemoveHandler(DragStopEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent DragStopEvent = EventManager.RegisterRoutedEvent(
            "DragStop", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProgressBarDrag));



        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler DragProcess
        {
            add { AddHandler(DragProcessEvent, value); }
            remove { RemoveHandler(DragProcessEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent DragProcessEvent = EventManager.RegisterRoutedEvent(
            "DragProcess", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProgressBarDrag));


        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler DragStart
        {
            add { AddHandler(DragStartEvent, value); }
            remove { RemoveHandler(DragStartEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent DragStartEvent = EventManager.RegisterRoutedEvent(
            "DragStart", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProgressBarDrag));


        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler PressedPosition
        {
            add { AddHandler(PressedPositionEvent, value); }
            remove { RemoveHandler(PressedPositionEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent PressedPositionEvent = EventManager.RegisterRoutedEvent(
            "PressedPosition", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProgressBarDrag));




        //public string ValueTime
        //{
        //    get { return (string)GetValue(ValueTimeProperty); }
        //    set { SetValue(ValueTimeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ValueTime.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ValueTimeProperty =
        //    DependencyProperty.Register("ValueTime", typeof(string), typeof(ProgressBarDragControl));

        //private string MinuteSecond(double second)
        //{
        //    TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(second));
        //    string str = "";
        //    if (ts.Minutes > 0)
        //    {
        //        str = ts.Minutes.ToString() + "分" + ts.Seconds + "秒";
        //    }
        //    if (ts.Minutes == 0)
        //    {
        //        str = ts.Seconds + "秒";
        //    }
        //    return str;
        //}
        //protected override void OnValueChanged(double oldValue, double newValue)
        //{
        //    base.OnValueChanged(oldValue, newValue);
        //    ValueTime = MinuteSecond(newValue);
        //}

    }
}
