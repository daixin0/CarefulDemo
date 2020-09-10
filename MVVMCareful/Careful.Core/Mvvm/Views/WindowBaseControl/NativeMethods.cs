using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Core.Mvvm.Views.WindowBaseControl
{
    public class NativeMethods
    {
        /// <summary> 
        /// 带有外边框和标题的windows的样式 
        /// </summary> 
        public const int WS_CAPTION = (int)0X00C0000L;

        // public const long WS_BORDER = 0X0080000L; 

        /// <summary> 
        /// window 扩展样式 分层显示 
        /// </summary> 
        public const int WS_EX_LAYERED = (int)0x00080000L;

        /// <summary> 
        /// 带有alpha的样式 
        /// </summary> 
        public const int LWA_ALPHA = (int)0x00000002L;

        /// <summary> 
        /// 颜色设置 
        /// </summary> 
        public const int LWA_COLORKEY = (int)0x00000001L;

        /// <summary> 
        /// window的基本样式 
        /// </summary> 
        public const int GWL_STYLE = -16;

        /// <summary> 
        /// window的扩展样式 
        /// </summary> 
        public const int GWL_EXSTYLE = -20;

        /// <summary> 
        /// 设置窗体的样式 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="oldStyle">进行设置窗体的样式类型.</param> 
        /// <param name="newStyle">新样式</param> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        //[DllImport("TwiHikVision.dll", EntryPoint = "SetWindowLong", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary> 
        /// 获取窗体指定的样式. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="style">要进行返回的样式</param> 
        /// <returns>当前window的样式</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr handle, int style);

        /// <summary> 
        /// 设置窗体的工作区域. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄.</param> 
        /// <param name="handleRegion">操作窗体区域的句柄.</param> 
        /// <param name="regraw">if set to <c>true</c> [regraw].</param> 
        /// <returns>返回值</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int SetWindowRgn(IntPtr handle, IntPtr handleRegion, bool regraw);

        /// <summary> 
        /// 创建带有圆角的区域. 
        /// </summary> 
        /// <param name="x1">左上角坐标的X值.</param> 
        /// <param name="y1">左上角坐标的Y值.</param> 
        /// <param name="x2">右下角坐标的X值.</param> 
        /// <param name="y2">右下角坐标的Y值.</param> 
        /// <param name="width">圆角椭圆的width.</param> 
        /// <param name="height">圆角椭圆的height.</param> 
        /// <returns>hRgn的句柄</returns> 
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int width, int height);

        /// <summary> 
        /// Sets the layered window attributes. 
        /// </summary> 
        /// <param name="handle">要进行操作的窗口句柄</param> 
        /// <param name="colorKey">RGB的值</param> 
        /// <param name="alpha">Alpha的值，透明度</param> 
        /// <param name="flags">附带参数</param> 
        /// <returns>true or false</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        //[DllImport("TwiHikVision.dll", EntryPoint = "SetLayeredWindowAttributes", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetLayeredWindowAttributes(IntPtr handle, int colorKey, byte alpha, int flags);


        public static void SetWindowRectanle(Window w)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(w).Handle;
            int oldstyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, oldstyle & ~WS_CAPTION);
            SetLayeredWindowAttributes(hwnd, 1 | 2 << 8 | 3 << 16, 0, LWA_ALPHA);

            SetWindowRgn(hwnd, CreateRoundRectRgn(0, 0, Convert.ToInt32(w.ActualWidth * 1.75), Convert.ToInt32(w.ActualHeight * 1.75), 6, 6), true);
        }
    }
}
