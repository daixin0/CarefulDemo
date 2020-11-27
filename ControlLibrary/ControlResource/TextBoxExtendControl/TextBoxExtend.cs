using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Careful.Controls.TextBoxExtendControl
{
    public class TextBoxExtend : TextBox
    {
        static TextBoxExtend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxExtend), new FrameworkPropertyMetadata(typeof(TextBoxExtend)));
        }

        protected override DependencyObject GetUIParentCore()
        {
            return base.GetUIParentCore();
        }

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(TextBoxExtend));



        public double PathWidth
        {
            get { return (double)GetValue(PathWidthProperty); }
            set { SetValue(PathWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathWidthProperty =
            DependencyProperty.Register("PathWidth", typeof(double), typeof(TextBoxExtend));



        public double PathHeight
        {
            get { return (double)GetValue(PathHeightProperty); }
            set { SetValue(PathHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathHeightProperty =
            DependencyProperty.Register("PathHeight", typeof(double), typeof(TextBoxExtend));



        public CornerRadius TextBoxCornerRadius
        {
            get { return (CornerRadius)GetValue(TextBoxCornerRadiusProperty); }
            set { SetValue(TextBoxCornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBoxCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxCornerRadiusProperty =
            DependencyProperty.Register("TextBoxCornerRadius", typeof(CornerRadius), typeof(TextBoxExtend), new PropertyMetadata(new CornerRadius(2)));



        public string WaterText
        {
            get { return (string)GetValue(WaterTextProperty); }
            set { SetValue(WaterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterTextProperty =
            DependencyProperty.Register("WaterText", typeof(string), typeof(TextBoxExtend));



        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AlignmentX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlignmentXProperty =
            DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(TextBoxExtend), new PropertyMetadata(AlignmentX.Left));



        public FontStyle WaterFontStyle
        {
            get { return (FontStyle)GetValue(WaterFontStyleProperty); }
            set { SetValue(WaterFontStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterFontStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterFontStyleProperty =
            DependencyProperty.Register("WaterFontStyle", typeof(FontStyle), typeof(TextBoxExtend), new PropertyMetadata(FontStyles.Normal));


    }
}
