using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Careful.Controls.NumberBoxControl
{
    public class NumericBoxControl : TextBox
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ControlTemplate ct = this.Template;
            Button btn_Up = ct.FindName("btn_Up", this) as Button;
            Button btn_Down = ct.FindName("btn_Down", this) as Button;
            btn_Up.Click += btnUp_Click;
            btn_Down.Click += btnDown_Click;

            this.Text = MinValue.ToString();

            this.PreviewTextInput -= NumericBoxControl_PreviewTextInput;
            this.PreviewTextInput += NumericBoxControl_PreviewTextInput;
            this.TextChanged -= NumericBoxControl_TextChanged;
            this.TextChanged += NumericBoxControl_TextChanged;
            this.PreviewKeyDown -= NumericBoxControl_PreviewKeyDown;
            this.PreviewKeyDown += NumericBoxControl_PreviewKeyDown;
        }

        private void NumericBoxControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PrevText))
                PrevText = Text;
            if (ValidateText(Text))
            {
                PrevText = Text;
            }
            else
            {
                Text = PrevText;
            }
        }

        private void NumericBoxControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (this.Text.Length > SelectionStart)
                    this.Text.Remove(SelectionStart, 1);
            }

            if (string.IsNullOrWhiteSpace(Text))
            {
                this.Text = MinValue.ToString();
            }
        }

        private string PrevText { get; set; }
        private void NumericBoxControl_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PrevText))
                PrevText = Text;
            if (ValidateText(Text))
            {
                PrevText = Text;
            }
            else
            {
                Text = PrevText;
            }
        }
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumericBoxControl), new PropertyMetadata(0));




        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericBoxControl), new PropertyMetadata(100));



        public bool IsExistRegion
        {
            get { return (bool)GetValue(IsExistRegionProperty); }
            set { SetValue(IsExistRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExistRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExistRegionProperty =
            DependencyProperty.Register("IsExistRegion", typeof(bool), typeof(NumericBoxControl), new PropertyMetadata(true));


        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (Text == string.Empty)
                Text = MinValue.ToString();
            int num = -1;
            if (!int.TryParse(Text, out num))
                return;
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
            int num = -1;
            if (!int.TryParse(Text, out num))
                return;
            if (IsExistRegion)
            {
                if (num <= MinValue)
                    return;
            }
            Text = (--num).ToString();
        }

        private bool ValidateText(string text)
        {
            double number;
            if (!Double.TryParse(text, out number))
            {
                return false;
            }
            if (number < MinValue)
            {
                return false;
            }
            if (number > MaxValue)
            {
                return false;
            }
            int dotPointer = text.IndexOf('.');
            if (dotPointer == -1)
            {
                return true;
            }
            return true;
        }

        protected override DependencyObject GetUIParentCore()
        {
            return base.GetUIParentCore();
        }

        public static readonly RoutedEvent ButtonClickEvent = EventManager.RegisterRoutedEvent
                     ("ButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(NumericBoxControl));

        public event RoutedEventHandler ButtonClick
        {
            add { this.AddHandler(ButtonClickEvent, value); }
            remove { this.RemoveHandler(ButtonClickEvent, value); }
        }


    }
}