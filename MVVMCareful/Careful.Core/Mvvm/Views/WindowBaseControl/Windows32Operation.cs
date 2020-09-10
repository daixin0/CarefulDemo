using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Careful.Core.Mvvm.Views.WindowBaseControl
{
    public class Windows32Operation
    {
        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("user32 ")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32 ")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);




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

            public int left;

            public int top;

            public int right;

            public int bottom;
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
            width = rec.right - rec.left;
            height = rec.bottom - rec.top;
            if(rec.left == 0 && rec.top == 0 && rec.bottom > rec.right)
            {
                return WindowsLocation.left;
            }
            else if(rec.left == 0 && rec.top != 0)
            {
                return WindowsLocation.bottom;
            }
            else if(rec.left !=0 && rec.top == 0)
            {
                return WindowsLocation.right;
            }
           else
            {
                return WindowsLocation.top;
            }
            
        }
    }
}
