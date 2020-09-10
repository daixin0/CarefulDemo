using Careful.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Careful.Core.AuthorizationManagement
{
    public class DynamicValue : DependencyObject
    {
        public object AuthData
        {
            get { return (object)GetValue(AuthDataProperty); }
            set { SetValue(AuthDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthDataProperty =
            DependencyProperty.Register("AuthData", typeof(object), typeof(DynamicValue), new PropertyMetadata(AuthData_changed));
        private static void AuthData_changed(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            DynamicValue data = (DynamicValue)element;
            data.AuthDataChanged();
        }
        public event EventHandler AuthChanged;
        private void AuthDataChanged()
        {
            AuthChanged?.Invoke(null, null);
        }

    }
    
}
