using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using WPFCommonLib.Base;
using WPFCommonLib.Utility;

namespace WPFCommonLib.Behaviors
{
    /// <summary>
    /// 用于打开指定窗口的行为(Action)，并可以在窗口关闭时执行指定方法或命令
    /// </summary>
    /// <remarks>
    /// 已知 BUG:
    /// 1) 当为 ContextMenu 中的 MenutItem 的 Click 事件设置此 Action 时，无法执行窗体关闭时的方法或命令
    /// </remarks>
    public class OpenWindowAction : TriggerAction<DependencyObject>
    {
        // Using a DependencyProperty as the backing store for ChenckingFuncBeforeOpenning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChenckingFuncBeforeOpenningProperty =
            DependencyProperty.Register("ChenckingFuncBeforeOpenning", typeof(Func<bool>), typeof(OpenWindowAction), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandAfterCloseProperty =
                    DependencyProperty.Register("CommandAfterClose", typeof(ICommand), typeof(OpenWindowAction), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandBeforeOpenProperty =
            DependencyProperty.Register("CommandBeforeOpen", typeof(ICommand), typeof(OpenWindowAction), new PropertyMetadata(null));

        public static readonly DependencyProperty IsModalProperty =
                            DependencyProperty.Register("IsModal", typeof(bool), typeof(OpenWindowAction), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for IsSingleton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSingletonProperty =
            DependencyProperty.Register("IsSingleton", typeof(bool), typeof(OpenWindowAction), new PropertyMetadata(false));

        public static readonly DependencyProperty MethodAfterCloseProperty =
                    DependencyProperty.Register("MethodAfterClose", typeof(string), typeof(OpenWindowAction), new PropertyMetadata(null));

        public static readonly DependencyProperty MethodBeforeOpenProperty =
            DependencyProperty.Register("MethodBeforeOpen", typeof(string), typeof(OpenWindowAction), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MethodOfTargetObjectProperty =
                    DependencyProperty.Register("MethodOfTargetObject", typeof(object), typeof(OpenWindowAction), new PropertyMetadata(null));

        public static readonly DependencyProperty ParameterProperty =
                                    DependencyProperty.Register("Parameter", typeof(object), typeof(OpenWindowAction), new PropertyMetadata(null));

        public static readonly DependencyProperty WindowTypeProperty =
                            DependencyProperty.Register("WindowType", typeof(Type), typeof(OpenWindowAction), new PropertyMetadata(null));

        public Func<bool> ChenckingFuncBeforeOpenning
        {
            get { return (Func<bool>)GetValue(ChenckingFuncBeforeOpenningProperty); }
            set { SetValue(ChenckingFuncBeforeOpenningProperty, value); }
        }

        /// <summary>
        /// 窗口关闭时要执行的命令
        /// </summary>
        public ICommand CommandAfterClose
        {
            get { return (ICommand)GetValue(CommandAfterCloseProperty); }
            set { SetValue(CommandAfterCloseProperty, value); }
        }

        public ICommand CommandBeforeOpen
        {
            get { return (ICommand)GetValue(CommandBeforeOpenProperty); }
            set { SetValue(CommandBeforeOpenProperty, value); }
        }

        /// <summary>
        /// 是否为模态窗口，默认为 true
        /// </summary>
        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        public bool IsSingleton
        {
            get { return (bool)GetValue(IsSingletonProperty); }
            set { SetValue(IsSingletonProperty, value); }
        }

        /// <summary>
        /// 窗口关闭后要执行的方法名
        /// </summary>
        public string MethodAfterClose
        {
            get { return (string)GetValue(MethodAfterCloseProperty); }
            set { SetValue(MethodAfterCloseProperty, value); }
        }

        /// <summary>
        /// 窗口打开之前要执行的方法名
        /// </summary>
        /// <remarks>该方法必须返回 Task 类型，这是为了弥补 <see cref="CommandBeforeOpen"/>不仅执行异步方法的不足；而 MethodAfterClose 属性所指向的方法目前无此要求</remarks>
        public string MethodBeforeOpen
        {
            get { return (string)GetValue(MethodBeforeOpenProperty); }
            set { SetValue(MethodBeforeOpenProperty, value); }
        }

        /// <summary>
        /// 窗口关闭后要执行的方法所属的对象（通常情况下是 ViewModel）
        /// </summary>
        public object MethodOfTargetObject
        {
            get { return (object)GetValue(MethodOfTargetObjectProperty); }
            set { SetValue(MethodOfTargetObjectProperty, value); }
        }

        /// <summary>
        /// 打开窗口时要传递的参数
        /// </summary>
        public object Parameter
        {
            get { return (object)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// 窗口类型
        /// </summary>
        public Type WindowType
        {
            get { return (Type)GetValue(WindowTypeProperty); }
            set { SetValue(WindowTypeProperty, value); }
        }

        protected override async void Invoke(object parameter)
        {
            try
            {
                if (IsSingleton)
                {
                    var targetWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(m => m.GetType() == WindowType);
                    if (targetWindow != null)
                    {
                        targetWindow.Activate();
                        return;
                    }
                }

                if (this.ChenckingFuncBeforeOpenning != null)
                {
                    if (!ChenckingFuncBeforeOpenning())
                    {
                        return;
                    }
                }

                if (CommandBeforeOpen != null)
                {
                    CommandBeforeOpen.Execute(null);
                }

                if (!string.IsNullOrWhiteSpace(MethodBeforeOpen))
                {
                    MethodInfo method = null;
                    Task task = null;

                    // 如果没有指定 MethodOfTargetObject，则默认为 AssociatedObject 的 DataContext (通常是 ViewModel)
                    if (MethodOfTargetObject == null && MethodOfTargetObject is FrameworkElement)
                    {
                        var dataContext = (MethodOfTargetObject as FrameworkElement).DataContext;
                        if (dataContext != null)
                        {
                            method = dataContext.GetType().GetMethod(MethodBeforeOpen, BindingFlags.Public | BindingFlags.Instance);
                            task = (Task)method?.Invoke(dataContext, null);
                        }
                    }
                    else
                    {
                        method = MethodOfTargetObject?.GetType().GetMethod(MethodBeforeOpen, BindingFlags.Public | BindingFlags.Instance);
                        task = (Task)method?.Invoke(MethodOfTargetObject, null);
                    }

                    if (task != null)
                    {
                        await task;
                    }
                }

                var windowObj = Activator.CreateInstance(WindowType);

                var window = windowObj as Window;

                window.Closed += (s, e) =>
                {
                    // 获取 Return Value
                    object returnValue = null;

                    if (window.DataContext != null)
                    {
                        if (window.DataContext is IWindowReturnValue)
                        {
                            returnValue = (window.DataContext as IWindowReturnValue).ReturnValue;
                        }
                        else
                        {
                            // 泛型类型
                            var geTypeInterface = window.DataContext.GetType().GetInterfaces()
                                .Where(t => t.IsGenericType)
                                .FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(IWindowReturnValue<>));

                            if (geTypeInterface != null)
                            {
                                returnValue = geTypeInterface.GetProperty(nameof(IWindowReturnValue.ReturnValue)).GetValue(window.DataContext, null);
                            }
                        }
                    }

                    // 先调用 Command（如果设置了），再调用方法（如果同样设置了）
                    // TODO: 这里临时这样做，如果没有返回值，则在调用 After Close 系列方法时，传入 Parameter 参数
                    //if (returnValue == null)
                    //{
                    //    returnValue = Parameter;
                    //}
                    CommandAfterClose?.Execute(returnValue);

                    // 再调用方法
                    if (!string.IsNullOrWhiteSpace(MethodAfterClose))
                    {
                        MethodInfo method = null;

                        // 如果没有指定 MethodOfTargetObject，则默认为 AssociatedObject 的 DataContext (通常是 ViewModel)
                        if (MethodOfTargetObject == null && MethodOfTargetObject is FrameworkElement)
                        {
                            var dataContext = (MethodOfTargetObject as FrameworkElement).DataContext;
                            if (dataContext != null)
                            {
                                method = dataContext.GetType().GetMethod(MethodAfterClose, BindingFlags.Public | BindingFlags.Instance);
                                method?.Invoke(dataContext, returnValue != null ? new object[] { returnValue } : null);
                            }
                        }
                        else
                        {
                            method = MethodOfTargetObject?.GetType().GetMethod(MethodAfterClose, BindingFlags.Public | BindingFlags.Instance);
                            method?.Invoke(MethodOfTargetObject, returnValue != null ? new object[] { returnValue } : null);
                        }
                    }
                };

                if (window.DataContext != null && window.DataContext is ViewModelBaseRoot)
                {
                    (window.DataContext as ViewModelBaseRoot).Data = Parameter;
                }
                else
                {
                    window.Tag = Parameter;
                }

                if (IsModal)
                {
                    window.Owner = ApplicationHelper.GetCurrentActivatedWindow();
                    window.ShowDialog();
                }
                else
                {
                    window.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}