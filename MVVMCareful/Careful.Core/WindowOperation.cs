using Careful.Core.DialogServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace Careful.Core
{
    public class WindowOperation
    {

        public static Window GetCurrentActivatedWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(m => m.IsActive);
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
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

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
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        ///<summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);


        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("user32 ")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        /// <summary>
        /// 0关闭窗口 1正常大小显示窗口 2最小化窗口 3最大化窗口  5显示窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public extern static IntPtr ShowWindow(IntPtr hWnd, uint nCmdShow);


        /// <summary>
        /// 获取窗口大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {

            public int cbSize;

            public IntPtr hWnd;

            public int uCallbackMessage;

            public int uEdge;//属性代表上、下、左、右

            public RECT rc;

            public IntPtr lParam;

        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {

            public int Left;

            public int Top;

            public int Right;

            public int Bottom;
        }

        public enum WindowsLocation
        {
            bottom,
            left,
            right,
            top,
        }
        public static WindowsLocation GetWindowsBarLocation(out int width, out int height)
        {
            width = 0;
            height = 0;
            int aaa = 0x00000005;
            APPBARDATA pdat = new APPBARDATA();
            SHAppBarMessage(aaa, ref pdat);
            int x = pdat.uEdge;
            RECT rec = pdat.rc;
            width = rec.Right - rec.Left;
            height = rec.Bottom - rec.Top;
            if (rec.Left == 0 && rec.Top == 0 && rec.Bottom > rec.Right)
            {
                return WindowsLocation.left;
            }
            else if (rec.Left == 0 && rec.Top != 0)
            {
                return WindowsLocation.bottom;
            }
            else if (rec.Left != 0 && rec.Top == 0)
            {
                return WindowsLocation.right;
            }
            else
            {
                return WindowsLocation.top;
            }

        }

        #endregion 激活进程中的窗口
    }
    public class WindowHandleManagement
    {
        private static Window _mainWindow;
        public static Window MainWindow
        {
            get { return _mainWindow; }
        }

        private static IntPtr OwnerWindow { get; set; }

        public static void HandleShowWindow(IntPtr owner, Window window, bool isWindowShow = false)
        {
            if (owner != null && owner.ToInt32() != 1)
                OwnerWindow = owner;
            if (!isWindowShow)
            {
                foreach (var item in Application.Current.Windows.OfType<Window>())
                {
                    if (item.GetType().Name == "AdornerWindow")
                        continue;
                    if (item.IsLoaded && item.IsVisible && PresentationSource.FromVisual(item) != null)
                    {
                        return;
                    }
                }
            }

            var targetWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(m => m.GetType() == window.GetType());
            if (targetWindow != null)
            {
                WindowOperation.SetParent(new WindowInteropHelper(window).Handle, owner);
                WindowOperation.ShowWindow(new WindowInteropHelper(window).Handle, 1);
                var windowHelper = new WindowInteropHelper(targetWindow)
                {
                    Owner = OwnerWindow
                };
                targetWindow.Closing -= MainWindow_Closing;
                targetWindow.Closing += MainWindow_Closing;
                targetWindow.Show();
            }
        }
        public static void HandleShowDialogWindow(IntPtr owner, Window window, bool isWindowShow = false, Action<IDialogResult> action = null)
        {
            if (owner != null && owner.ToInt32() != 1)
                OwnerWindow = owner;
            if (!isWindowShow)
            {
                foreach (var item in Application.Current.Windows.OfType<Window>())
                {
                    if (item.GetType().Name == "AdornerWindow")
                        continue;
                    if (item.IsLoaded && item.IsVisible && PresentationSource.FromVisual(item) != null)
                    {
                        return;
                    }
                }
            }

            var targetWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(m => m.GetType() == window.GetType());
            if (targetWindow != null)
            {
                WindowOperation.SetParent(new WindowInteropHelper(window).Handle, owner);
                WindowOperation.ShowWindow(new WindowInteropHelper(window).Handle, 1);
                var windowHelper = new WindowInteropHelper(targetWindow)
                {
                    Owner = OwnerWindow
                };
                targetWindow.Closing -= TargetWindow_Closing;
                targetWindow.Closing += TargetWindow_Closing;
                targetWindow.ShowDialog();
                if (targetWindow.DataContext is IDialogResult dialog)
                    action?.Invoke(dialog);
                else
                    action?.Invoke(null);
            }
        }
        public static void HandleShowMessageDialogWindow(IntPtr owner, Window window)
        {
            OwnerWindow = owner;
            WindowOperation.SetParent(new WindowInteropHelper(window).Handle, owner);
            WindowOperation.ShowWindow(new WindowInteropHelper(window).Handle, 1);
            var windowHelper = new WindowInteropHelper(window)
            {
                Owner = owner
            };
            window.Closing -= Window_Closing;
            window.Closing += Window_Closing;
            window.ShowDialog();
        }
        public static void HandleShowMessageWindow(IntPtr owner, Window window)
        {
            OwnerWindow = owner;
            WindowOperation.SetParent(new WindowInteropHelper(window).Handle, owner);
            WindowOperation.ShowWindow(new WindowInteropHelper(window).Handle, 1);
            var windowHelper = new WindowInteropHelper(window)
            {
                Owner = owner
            };
            window.Closing -= Window_Closing;
            window.Closing += Window_Closing;
            window.Show();
        }
        private static void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowOperation.SetForegroundWindow(OwnerWindow);
        }

        private static void TargetWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindow = null;
            WindowOperation.SetForegroundWindow(OwnerWindow);
        }

        private static void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindow = null;
            WindowOperation.SetForegroundWindow(OwnerWindow);
        }

    }
}