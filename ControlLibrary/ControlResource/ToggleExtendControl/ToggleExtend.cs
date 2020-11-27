using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Careful.Controls.ToggleExtendControl
{
    public class ToggleExtend : ToggleButton
    {


        public object PressedContent
        {
            get { return (object)GetValue(PressedContentProperty); }
            set { SetValue(PressedContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedContentProperty =
            DependencyProperty.Register("PressedContent", typeof(object), typeof(ToggleExtend));

    }
}
