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
using static Careful.Core.Win32Helper;

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

		public enum WindowsLocation
		{
			Bottom,
			Left,
			Right,
			Top,
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
				return WindowsLocation.Left;
			}
			else if (rec.Left == 0 && rec.Top != 0)
			{
				return WindowsLocation.Bottom;
			}
			else if (rec.Left != 0 && rec.Top == 0)
			{
				return WindowsLocation.Right;
			}
			else
			{
				return WindowsLocation.Top;
			}

		}
	}
    
}