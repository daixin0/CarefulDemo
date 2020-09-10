using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using WPFCommonLib;
using WPFCommonLib.Extensions;

namespace WPFCommonLib.Helpers
{
    public class ApplicationHelper
    {
       
        public static Window GetCurrentActivatedWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(m => m.IsActive);
        }

        public static Type GetTargetType(string fullQulifiedName, Func<Assembly, bool> condition = null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().AsEnumerable();
            if (condition != null)
            {
                assemblies = assemblies.Where(condition);
            }

            Type type = null;
            try
            {
                type = assemblies?.SelectMany(a => a.GetTypes()).SingleOrDefault(t => t.FullName == fullQulifiedName);
            }
            catch (Exception ex)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var loadException = ex as ReflectionTypeLoadException;

                    if (loadException.LoaderExceptions.HasItems())
                    {
                        StringBuilder sb = new StringBuilder();
                        StringBuilder sbTitle = new StringBuilder();
                        foreach (var item in loadException.LoaderExceptions.ToList())
                        {
                            sb.AppendLine(item.ToString());
                            sbTitle.AppendLine(item.Message);
                        }

                        throw new Exception("获取已加载程序集信息时出错，详细信息:" + Environment.NewLine + sbTitle.ToString(), new Exception(sb.ToString()));
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw new Exception("获取已加载程序集信息时出错", ex);
                }
            }

            //if (type == null)
            //{
            //    throw new Exception("找不到指定的 Type: " + fullQulifiedName);
            //}

            return type;
        }

        public static T GetWindow<T>() where T : Window
        {
            return Application.Current.Windows.OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetWindows<T>() where T : Window
        {
            return Application.Current.Windows.OfType<T>();
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        #region 激活进程中的窗口

        private const int SW_SHOWNOMAL = 1;

        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//显示
            SetForegroundWindow(instance.MainWindowHandle);//当到最前端
        }

        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        ///<summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        #endregion 激活进程中的窗口
    }
}