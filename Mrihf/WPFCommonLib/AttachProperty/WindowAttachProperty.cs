using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WPFCommonLib.Helpers;
using WPFCommonLib.Utility;
using WPFCommonLib.Views;

namespace WPFCommonLib.AttachProperty
{
    public class WindowAttachProperty: DependencyObject
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
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(WindowAttachProperty), new PropertyMetadata(OnDialogResultChanged));

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Window))
            {
                return;
            }
            var window = d as Window;
            if ((bool?)e.NewValue == true)
            {
                window.DialogResult = true;
            }
            else if ((bool?)e.NewValue == false)
            {
                window.Close();
            }
        }


        public static readonly DependencyProperty CommandAfterCloseProperty =
           DependencyProperty.RegisterAttached("CommandAfterClose", typeof(ICommand), typeof(WindowAttachProperty), new PropertyMetadata(null));

        public static readonly DependencyProperty IsModalProperty =
                    DependencyProperty.RegisterAttached("IsModal", typeof(bool), typeof(WindowAttachProperty), new PropertyMetadata(true));

        public static readonly DependencyProperty OpenWindowTypeProperty =
                            DependencyProperty.RegisterAttached("OpenWindowType", typeof(Type), typeof(WindowAttachProperty), new PropertyMetadata(null, OnOpenWindowTypeChanged));

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.RegisterAttached("Parameter", typeof(object), typeof(WindowAttachProperty), new PropertyMetadata(null));

        public static ICommand GetCommandAfterClose(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandAfterCloseProperty);
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


        private static void OnOpenWindowTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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
                bool? result = null;

                window.Closed += (win, closeArgs) =>
                {
                    var command = GetCommandAfterClose(d);
                    if (command != null)
                    {
                        command.Execute((win as Window).DialogResult);
                    }

                    window = null;
                };

                if (isModel)
                {
                    FrameWindow frameWindow = null;
                    bool isMainWindow = ApplicationHelper.GetCurrentActivatedWindow() is FrameWindow;
                    if (isMainWindow)
                    {
                        frameWindow = ApplicationHelper.GetCurrentActivatedWindow() as FrameWindow;
                        frameWindow.WindowContainer.ShowMask = true;
                    }
                    window.Owner = ApplicationHelper.GetCurrentActivatedWindow();
                    result = window.ShowDialog();
                    if (isMainWindow)
                    {
                        frameWindow.WindowContainer.ShowMask = false;
                    }
                }
                else
                {
                    window.Show();
                }

                

                
            });

            control.Click += clickEventHandler;
        }
    }
}
