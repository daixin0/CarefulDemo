using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Careful.Controls.ComboBoxExtendControl
{
    public class ComboBoxExtend : ComboBox
    {


        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AlignmentX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlignmentXProperty =
            DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(ComboBoxExtend), new PropertyMetadata(AlignmentX.Left));


        public string WaterText
        {
            get { return (string)GetValue(WaterTextProperty); }
            set { SetValue(WaterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterTextProperty =
            DependencyProperty.Register("WaterText", typeof(string), typeof(ComboBoxExtend));


    }
}
