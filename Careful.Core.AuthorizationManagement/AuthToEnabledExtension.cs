using Careful.Core.Mvvm;
using Careful.Core.Mvvm.BindingExtension;
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
    [MarkupExtensionReturnType(typeof(bool))]
    public class AuthToEnabledExtension : UpdatableMarkupExtension
    {
        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            string authName = "";
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                authName = GetAuthName();
            }));
            if (String.IsNullOrEmpty(authName))
                return false;

            return AuthProvider.Instance.CheckAccess(authName);
        }
        private string GetAuthName()
        {
            if (!(TargetObject is FrameworkElement framework))
                return null;
            object obj = framework.DataContext;
            Type targetType = obj.GetType();
            PropertyInfo propertyInfo = targetType.GetProperty(Path.Path);
            string authInfo = propertyInfo.GetValue(obj)?.ToString();
            string authName = Converter.Convert(authInfo, typeof(string), ConverterParameter, ConverterCulture).ToString();
            NotifyPropertyChanged notifyObject = obj as NotifyPropertyChanged;
            notifyObject.PropertyChanged += NotifyObject_PropertyChanged;
            return authName;
        }

        private void NotifyObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string authName = "";
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                authName = GetAuthName();
            }));
            if (String.IsNullOrEmpty(authName))
                UpdateValue(false);

            UpdateValue(AuthProvider.Instance.CheckAccess(authName));
        }
    }
}
