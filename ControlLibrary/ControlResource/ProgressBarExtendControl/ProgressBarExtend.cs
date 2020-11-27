using System.Windows;
using System.Windows.Controls;

namespace Careful.Controls.ProgressBarExtendControl
{
    public class ProgressBarExtend : ProgressBar
    {

        public CornerRadius ProgressCornerRadius
        {
            get { return (CornerRadius)GetValue(ProgressCornerRadiusProperty); }
            set { SetValue(ProgressCornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressCornerRadiusProperty =
            DependencyProperty.Register("ProgressCornerRadius", typeof(CornerRadius), typeof(ProgressBarExtend));


    }
}
