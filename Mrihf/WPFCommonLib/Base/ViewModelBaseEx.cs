using System;
using System.Linq;
using System.Windows;
using WPFCommonLib.Utility;

namespace WPFCommonLib.Base
{
    public class ViewModelBaseEx : ViewModelBase, IOpenWindow, IOpenWindow2
    {
        public virtual void ShowWindow(OpenWindowInfo openWindowInfo)
        {
            if (openWindowInfo == null || openWindowInfo.WindowType == null)
            {
                throw new ArgumentNullException(nameof(openWindowInfo), "WindowType cannot be null");
            }

            Window window = null;
            object windowObj = null;

            try
            {
                windowObj = Activator.CreateInstance(openWindowInfo.WindowType);
                window = windowObj as Window;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Cannot create a window with the given type", ex);
            }

            if (openWindowInfo.Parameter != null && window.DataContext != null
                    && window.DataContext is ViewModelBaseRoot)
            {
                (window.DataContext as ViewModelBaseRoot).Data = openWindowInfo.Parameter;
            }

            if (openWindowInfo.IsModal)
            {
                window.Owner = ApplicationHelper.GetCurrentActivatedWindow();

                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        public virtual void ShowWindow(OpenWindowInfo openWindowInfo, Action<object> action)
        {
            if (openWindowInfo == null || openWindowInfo.WindowType == null)
            {
                throw new ArgumentNullException(nameof(openWindowInfo), "WindowType cannot be null");
            }

            Window window = null;
            object windowObj = null;

            try
            {
                windowObj = Activator.CreateInstance(openWindowInfo.WindowType);
                window = windowObj as Window;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Cannot create a window with the given type", ex);
            }

            if (openWindowInfo.Parameter != null && window.DataContext != null
                && window.DataContext is ViewModelBaseRoot)
            {
                (window.DataContext as ViewModelBaseRoot).Data = openWindowInfo.Parameter;
            }

            EventHandler closeEventHanlder = (s, e) =>
            {
                var vmType = window.DataContext.GetType();
                var geType = typeof(IWindowReturnValue<>);

                if (vmType is IWindowReturnValue)
                {
                    var value = (window.DataContext as IWindowReturnValue).ReturnValue;
                    action?.Invoke(value);
                }
                else
                {
                    // generic interface
                    var geTypeInterface = vmType.GetInterfaces()
                        .Where(t => t.IsGenericType)
                        .FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(IWindowReturnValue<>));

                    if (geTypeInterface != null)
                    {
                        var value = geTypeInterface.GetProperty(nameof(IWindowReturnValue.ReturnValue)).GetValue(window.DataContext, null);
                        action?.Invoke(value);
                    }
                }
            };

            window.Closed -= closeEventHanlder;
            window.Closed += closeEventHanlder;

            if (openWindowInfo.IsModal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }
    }

    public class ViewModelBaseEx<TReturnValue> : ViewModelBaseEx, IOpenWindow, IOpenWindow2<TReturnValue>
    {
        public virtual void ShowWindow(OpenWindowInfo openWindowInfo, Action<TReturnValue> action)
        {
            if (openWindowInfo == null || openWindowInfo.WindowType == null)
            {
                throw new ArgumentNullException(nameof(openWindowInfo), "WindowType cannot be null");
            }

            Window window = null;
            object windowObj = null;

            try
            {
                windowObj = Activator.CreateInstance(openWindowInfo.WindowType);
                window = windowObj as Window;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Cannot create a window with the given type", ex);
            }

            if (openWindowInfo.Parameter != null && window.DataContext != null
                && window.DataContext is ViewModelBaseRoot)
            {
                (window.DataContext as ViewModelBaseRoot).Data = openWindowInfo.Parameter;
            }

            EventHandler closeEventHanlder = (s, e) =>
            {
                var vmType = window.DataContext.GetType();
                var geType = typeof(IWindowReturnValue<>);

                if (vmType is IWindowReturnValue)
                {
                    var value = (window.DataContext as IWindowReturnValue).ReturnValue;
                    if (value != null)
                    {
                        action?.Invoke((TReturnValue)value);
                    }
                    else
                    {
                        action?.Invoke(default(TReturnValue));
                    }
                }
                else
                {
                    // generic interface
                    var geTypeInterface = vmType.GetInterfaces()
                        .Where(t => t.IsGenericType)
                        .FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(IWindowReturnValue<>));

                    if (geTypeInterface != null)
                    {
                        var value = geTypeInterface.GetProperty(nameof(IWindowReturnValue.ReturnValue)).GetValue(window.DataContext, null);
                        if (value != null)
                        {
                            action?.Invoke((TReturnValue)value);
                        }
                        else
                        {
                            action?.Invoke(default(TReturnValue));
                        }
                    }
                }
            };

            window.Closed -= closeEventHanlder;
            window.Closed += closeEventHanlder;

            if (openWindowInfo.IsModal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }
    }
}