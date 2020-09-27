using Careful.Core.Mvvm.BindingExtension;
using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Careful.Core.AuthorizationManagement
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class AuthToVisibilityExtension : UpdatableMarkupExtension
    {
        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            string authName = "";
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                authName = GetAuthName();
            }));
            if (String.IsNullOrEmpty(authName))
                return Visibility.Collapsed;

            if (AuthProvider.Instance.CheckAccess(authName))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
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
            notifyObject.PropertyChanged -= NotifyObject_PropertyChanged;
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
                UpdateValue(Visibility.Collapsed);

            if (AuthProvider.Instance.CheckAccess(authName))
                UpdateValue(Visibility.Visible);
            else
                UpdateValue(Visibility.Collapsed);
        }
    }
}
