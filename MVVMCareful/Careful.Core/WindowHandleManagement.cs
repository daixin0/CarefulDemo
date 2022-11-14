using Careful.Core.DialogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Careful.Core
{
    public static class WindowHandleManagement
    {
        private static Window _mainWindow;

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
                Win32Helper.SetParent(new WindowInteropHelper(window).Handle, owner);
                Win32Helper.ShowWindow(new WindowInteropHelper(window).Handle, 1);
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
                Win32Helper.SetParent(new WindowInteropHelper(window).Handle, owner);
                Win32Helper.ShowWindow(new WindowInteropHelper(window).Handle, 1);
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
            Win32Helper.SetParent(new WindowInteropHelper(window).Handle, owner);
            Win32Helper.ShowWindow(new WindowInteropHelper(window).Handle, 1);
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
            Win32Helper.SetParent(new WindowInteropHelper(window).Handle, owner);
            Win32Helper.ShowWindow(new WindowInteropHelper(window).Handle, 1);
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
            Win32Helper.SetForegroundWindow(OwnerWindow);
        }

        private static void TargetWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindow = null;
            Win32Helper.SetForegroundWindow(OwnerWindow);
        }

        private static void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindow = null;
            Win32Helper.SetForegroundWindow(OwnerWindow);
        }
        public static bool IsAttachedToPresentationSource(this Visual element)
        {
            return PresentationSource.FromVisual(element) != null;
        }

        public static void SetParentToMainWindowOf(this Window window, Visual element)
        {
            var wndParent = Window.GetWindow(element);
            if (wndParent != null)
                window.Owner = wndParent;
            else
            {
                if (GetParentWindowHandle(element, out IntPtr parentHwnd))
                    Win32Helper.SetOwner(new WindowInteropHelper(window).Handle, parentHwnd);
            }
        }

        public static IntPtr GetParentWindowHandle(this Window window)
        {
            if (window.Owner != null)
                return new WindowInteropHelper(window.Owner).Handle;
            else
                return Win32Helper.GetOwner(new WindowInteropHelper(window).Handle);
        }

        public static bool GetParentWindowHandle(this Visual element, out IntPtr hwnd)
        {
            hwnd = IntPtr.Zero;

            if (!(PresentationSource.FromVisual(element) is HwndSource wpfHandle))
                return false;

            hwnd = Win32Helper.GetParent(wpfHandle.Handle);
            if (hwnd == IntPtr.Zero)
                hwnd = wpfHandle.Handle;
            return true;
        }

        public static void SetParentWindowToNull(this Window window)
        {
            if (window.Owner != null)
                window.Owner = null;
            else
            {
                Win32Helper.SetOwner(new WindowInteropHelper(window).Handle, IntPtr.Zero);
            }
        }
    }
}
