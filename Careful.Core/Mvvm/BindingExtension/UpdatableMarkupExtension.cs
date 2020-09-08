using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Careful.Core.Mvvm.BindingExtension
{
    public abstract class UpdatableMarkupExtension : BindingMarkupExtensionBase
    {
        private object _targetObject;
        private object _targetProperty;

        protected object TargetObject
        {
            get { return _targetObject; }
        }

        protected object TargetProperty
        {
            get { return _targetProperty; }
        }

        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (target != null)
            {
                _targetObject = target.TargetObject;
                _targetProperty = target.TargetProperty;
            }

            return ProvideValueInternal(serviceProvider);
        }

        protected void UpdateValue(object value)
        {
            if (_targetObject != null)
            {
                if (_targetProperty is DependencyProperty)
                {
                    DependencyObject obj = _targetObject as DependencyObject;
                    DependencyProperty prop = _targetProperty as DependencyProperty;

                    Action updateAction = () => obj.SetValue(prop, value);

                    if (obj.CheckAccess())
                        updateAction();
                    else
                        obj.Dispatcher.Invoke(updateAction);
                }
                else
                {
                    PropertyInfo prop = _targetProperty as PropertyInfo;
                    prop.SetValue(_targetObject, value, null);
                }
            }
        }

        protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
    }
}
