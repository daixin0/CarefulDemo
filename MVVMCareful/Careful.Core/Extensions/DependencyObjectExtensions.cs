using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Careful.Core.Extensions
{
    public static partial class DependencyObjectExtensions
    {
        public static bool HasBinding(this FrameworkElement instance, DependencyProperty property)
            => BindingOperations.GetBinding(instance, property) != null;

        /// <summary>
        /// 延迟获得焦点，默认延迟 100ms
        /// </summary>
        /// <param name="p_element"></param>
        /// <param name="p_time"></param>
        public static void FocusDelay(this UIElement p_element, int p_time = 100)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(p_time);

                p_element.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => p_element.Focus()));
            });
        }

        /// <summary>
        /// 以类型获取第一子节点
        /// </summary>
        /// <typeparam name="T"/><peparam/>
        /// <param name="p_element"></param>
        /// <param name="p_func"></param>
        /// <returns></returns>
        public static T GetChild<T>(this DependencyObject p_element, Func<T, bool> p_func = null) where T : UIElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(p_element); i++)
            {
                UIElement child = VisualTreeHelper.GetChild(p_element, i) as FrameworkElement;

                if (child == null)
                {
                    continue;
                }

                var t = child as T;

                if (t != null && (p_func == null || p_func(t)))
                {
                    return (T)child;
                }

                var grandChild = child.GetChild(p_func);
                if (grandChild != null)
                    return grandChild;
            }

            return null;
        }

        /// <summary>
        /// 获取当前控件树下所在 T 类型节点 （深度遍历）
        /// </summary>
        /// <typeparam name="T"/><peparam/>
        /// <param name="p_element"></param>
        /// <param name="p_func"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetChildren<T>(this DependencyObject p_element, Func<T, bool> p_func = null) where T : UIElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(p_element); i++)
            {
                UIElement child = VisualTreeHelper.GetChild(p_element, i) as FrameworkElement;
                if (child == null)
                {
                    continue;
                }

                if (child is T)
                {
                    var t = (T)child;
                    if (p_func != null && !p_func(t))
                    {
                        continue;
                    }

                    yield return t;
                }
                else
                {
                    foreach (var c in child.GetChildren(p_func))
                    {
                        yield return c;
                    }
                }
            }
        }

        public static T GetParent<T>(this DependencyObject element) where T : DependencyObject
        {
            try
            {
                if (element == null) return null;
                DependencyObject parent = VisualTreeHelper.GetParent(element);
                while ((parent != null) && !(parent is T))
                {
                    DependencyObject newVisualParent = VisualTreeHelper.GetParent(parent);
                    if (newVisualParent != null)
                    {
                        parent = newVisualParent;
                    }
                    else
                    {
                        // try to get the logical parent ( e.g. if in Popup)
                        if (parent is FrameworkElement)
                        {
                            parent = (parent as FrameworkElement).Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return parent as T;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T GetParent<T>(this DependencyObject element, Func<T, bool> p_func) where T : DependencyObject
        {
            try
            {
                if (element == null) return null;
                DependencyObject parent = VisualTreeHelper.GetParent(element);
                while (((parent != null) && !(parent is T)) || !p_func(parent as T))
                {
                    DependencyObject newVisualParent = VisualTreeHelper.GetParent(parent);
                    if (newVisualParent != null)
                    {
                        parent = newVisualParent;
                    }
                    else
                    {
                        // try to get the logical parent ( e.g. if in Popup)
                        if (parent is FrameworkElement)
                        {
                            parent = (parent as FrameworkElement).Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return parent as T;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取UIElement的相对位置
        /// </summary>
        /// <param name="source"></param>
        /// <param name="p_relative">相对于UIElement</param>
        /// <returns>坐标值</returns>
        public static Point GetRelativePosition(this UIElement source, UIElement p_relative)
        {
            Point pt = new Point();
            MatrixTransform mat = source.TransformToVisual(p_relative) as MatrixTransform;
            if (mat != null)
            {
                pt.X = mat.Matrix.OffsetX;
                pt.Y = mat.Matrix.OffsetY;
            }
            return pt;
        }
    }
}
