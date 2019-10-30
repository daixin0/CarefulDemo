using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WPFCommonLib.Extensions;
using WPFCommonLib.Utility;

namespace WPFCommonLib.AttachProperty
{
    public class WindowHelper : DependencyObject
    {

        #region IsChildWindow

        public static bool GetIsChildWindow(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsChildWindowProperty);
        }

        public static void SetIsChildWindow(DependencyObject obj, bool value)
        {
            obj.SetValue(IsChildWindowProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsChildWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChildWindowProperty =
            DependencyProperty.RegisterAttached("IsChildWindow", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, OnIsChildWindowChanged));

        private static void OnIsChildWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                window.ShowInTaskbar = false;
                window.Owner = ApplicationHelper.GetCurrentActivatedWindow();
            }
            else
            {
                window.Owner = null;
                window.ShowInTaskbar = true;
            }
        }
        #endregion

        public static readonly DependencyProperty CommandAfterCloseProperty =
            DependencyProperty.RegisterAttached("CommandAfterClose", typeof(ICommand), typeof(WindowHelper), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Dragble.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragbleProperty =
            DependencyProperty.RegisterAttached("Dragble", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, OnDragableChanged));

        public static readonly DependencyProperty IsModalProperty =
                    DependencyProperty.RegisterAttached("IsModal", typeof(bool), typeof(WindowHelper), new PropertyMetadata(true));

        public static readonly DependencyProperty OpenWindowTypeProperty =
                            DependencyProperty.RegisterAttached("OpenWindowType", typeof(Type), typeof(WindowHelper), new PropertyMetadata(null, OnOpenWindowTypeChanged));

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.RegisterAttached("Parameter", typeof(object), typeof(WindowHelper), new PropertyMetadata(null));

        public static ICommand GetCommandAfterClose(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandAfterCloseProperty);
        }

        public static bool GetDragble(DependencyObject obj)
        {
            return (bool)obj.GetValue(DragbleProperty);
        }

        public static bool GetIsModal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsModalProperty);
        }

        public static Type GetOpenWindowType(DependencyObject obj)
        {
            return (Type)obj.GetValue(OpenWindowTypeProperty);
        }

        public static object GetParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(ParameterProperty);
        }

        public static void SetCommandAfterClose(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandAfterCloseProperty, value);
        }

        public static void SetDragble(DependencyObject obj, bool value)
        {
            obj.SetValue(DragbleProperty, value);
        }

        public static void SetIsModal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsModalProperty, value);
        }

        public static void SetOpenWindowType(DependencyObject obj, Type value)
        {
            obj.SetValue(OpenWindowTypeProperty, value);
        }

        public static void SetParameter(DependencyObject obj, object value)
        {
            obj.SetValue(ParameterProperty, value);
        }

        private static void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var element = sender as FrameworkElement;
                var window = element.GetParent<Window>();
                window.DragMove();
            }
        }

        private static void OnDragableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if ((bool)e.NewValue)
            {
                element.MouseMove += Element_MouseMove;
            }
            else
            {
                element.MouseMove -= Element_MouseMove;
            }
        }

        private static void OnOpenWindowTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 对于 Button、Hyperlink、MenuItem均适合
            dynamic control = null;
            switch (d.GetType().Name.ToString())
            {
                case "Button":
                    control = d as Button;
                    break;

                case "Hyperlink":
                    control = d as Hyperlink;
                    break;

                case "MenuItem":
                    control = d as MenuItem;
                    break;

                default:
                    return;
            }

            #region 旧代码

            // 对于 Button 和 Hyperlink 均适合
            //Button button = d as Button;
            //Hyperlink link = null;
            //if (button == null)
            //{
            //    if (d is Hyperlink)
            //    {
            //        link = d as Hyperlink;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            #endregion 旧代码

            var type = GetOpenWindowType(d);
            if (type == null && type != typeof(Window))
            {
                return;
            }

            Window window = null;
            var clickEventHandler = new RoutedEventHandler((s, arg) =>
             {
                 if (window == null)
                 {
                     window = Activator.CreateInstance(type) as Window;

                     if (window == null)
                     {
                         throw new ArgumentException("cannot create a window by the target type");
                     }
                 }

                 if (GetParameter(d) != null)
                 {
                     window.Tag = GetParameter(d);
                 }

                 var isModel = GetIsModal(d);

                 window.Closed += (win, closeArgs) =>
                 {
                     var command = GetCommandAfterClose(d);
                     if (command != null)
                     {
                         command.Execute(null);
                     }

                     // 关闭后，将对象置空；如果再次要打开时，再创建新实例
                     window = null;
                 };

                 if (isModel)
                 {
                     window.Owner = ApplicationHelper.GetCurrentActivatedWindow();
                     window.ShowDialog();
                 }
                 else
                 {
                     window.Show();
                 }
             });

            #region 旧代码

            //if (button != null)
            //{
            //    button.Click += clickEventHandler;
            //}
            //else if (link != null)
            //{
            //    link.Click += clickEventHandler;
            //}

            #endregion 旧代码

            control.Click += clickEventHandler;
        }
    }


}