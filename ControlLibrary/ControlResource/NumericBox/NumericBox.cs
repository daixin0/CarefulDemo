using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Careful.Controls.NumericBoxControl
{
    public class NumericBox : TextBox
    {
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ControlTemplate ct = this.Template;
            //Image lbl = ct.FindName("lblLeftMouseDown", this) as Image;
            //lbl.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
            Button btn_Up = ct.FindName("btn_Up", this) as Button;
            Button btn_Down = ct.FindName("btn_Down", this) as Button;
            btn_Up.Click += btnUp_Click;
            btn_Down.Click += btnDown_Click;
            //this.TextChanged += WaterTextBoxSearchControl_TextChanged;
            this.KeyDown += WaterTextBoxSearchControl_KeyDown;
            this.Text = MinValue.ToString();
        }


        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumericBox), new PropertyMetadata(1));




        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericBox), new PropertyMetadata(10));



        public bool IsExistRegion
        {
            get { return (bool)GetValue(IsExistRegionProperty); }
            set { SetValue(IsExistRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExistRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExistRegionProperty =
            DependencyProperty.Register("IsExistRegion", typeof(bool), typeof(NumericBox), new PropertyMetadata(true));



        private void WaterTextBoxSearchControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        private void WaterTextBoxSearchControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int.Parse(Text);
            }
            catch (Exception)
            {
                Text = "";
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (Text == string.Empty)
                Text = MinValue.ToString();
            long num = Convert.ToInt64(Text);
            if (IsExistRegion)
            {
                if (num >= MaxValue)
                    return;
            }
            Text = (++num).ToString();
        }
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (Text == string.Empty)
                Text = MinValue.ToString();
            long num = Convert.ToInt64(Text);
            if (IsExistRegion)
            {
                if (num <= MinValue)
                    return;
            }
            Text = (--num).ToString();
        }
        protected override DependencyObject GetUIParentCore()
        {
            return base.GetUIParentCore();
        }

        public static readonly RoutedEvent ButtonClickEvent = EventManager.RegisterRoutedEvent
                     ("ButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(NumericBox));

        public event RoutedEventHandler ButtonClick
        {
            add { this.AddHandler(ButtonClickEvent, value); }
            remove { this.RemoveHandler(ButtonClickEvent, value); }
        }


    }
}