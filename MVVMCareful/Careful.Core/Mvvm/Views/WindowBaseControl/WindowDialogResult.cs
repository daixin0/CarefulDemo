using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Core.Mvvm.Views.WindowBaseControl
{
    public class WindowDialogResult
    {


        public static bool? GetDialogResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(DialogResultProperty, value);
        }

        // Using a DependencyProperty as the backing store for DialogResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(WindowDialogResult), new PropertyMetadata(DialogResultChanged));


        private static void DialogResultChanged(DependencyObject sender,DependencyPropertyChangedEventArgs dependencyProperty)
        {
            if(sender is Window window)
            {
                if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                    window.DialogResult = GetDialogResult(sender);
                else
                {
                    window.Close();
                }
            }
        }


    }
}
