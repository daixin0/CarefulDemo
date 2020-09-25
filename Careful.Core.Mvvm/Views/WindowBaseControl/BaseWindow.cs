using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Careful.Core.Mvvm.Views.WindowBaseControl
{

    public class BaseWindow : Window, INotifyPropertyChanged, IView
    {
        ResourceDictionary style1;
        //static BaseWindow()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        //}
        public BaseWindow()
        {
            style1 = new ResourceDictionary();
            style1.Source = new Uri("Careful.Core;component/Mvvm/Views/WindowBaseControl/BaseWindowStyle.xaml", UriKind.Relative);
            this.Style = (System.Windows.Style)style1["BaseWindowStyle"];
            this.DataContext = this;
            this.StateChanged += BaseWindow_StateChanged;
        }

        private void BaseWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                SetWorkArea();
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
            }
            else if (WindowState == WindowState.Normal)
            {
                this.Top = NormalRect.Top;
                this.Left = NormalRect.Left;
                this.Width = NormalRect.Width;
                this.Height = NormalRect.Height;
                this.ResizeMode = System.Windows.ResizeMode.CanResize;
            }
        }




        public bool IsMinButton
        {
            get { return (bool)GetValue(IsMinButtonProperty); }
            set { SetValue(IsMinButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMinButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMinButtonProperty =
            DependencyProperty.Register("IsMinButton", typeof(bool), typeof(BaseWindow));



        public bool IsMaxButton
        {
            get { return (bool)GetValue(IsMaxButtonProperty); }
            set { SetValue(IsMaxButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMaxButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMaxButtonProperty =
            DependencyProperty.Register("IsMaxButton", typeof(bool), typeof(BaseWindow));

        public CornerRadius WindowCornerRadius
        {
            get { return (CornerRadius)GetValue(WindowCornerRadiusProperty); }
            set { SetValue(WindowCornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowCornerRadiusProperty =
            DependencyProperty.Register("WindowCornerRadius", typeof(CornerRadius), typeof(BaseWindow));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeEvent();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            //NativeMethods.SetWindowRectanle(this);
        }

        /// <summary>
        /// 初始化主窗体功能按钮事件加载
        /// </summary>
        private void InitializeEvent()
        {
            this.MouseMove += BaseWindow_MouseMove;
            ControlTemplate baseWindowTemplate = (ControlTemplate)style1["BaseWindowControlTemplate"];

            #region 窗体操作

            Button minBtn = (Button)baseWindowTemplate.FindName("btnMin", this);
            minBtn.Click += delegate
            {
                WindowState = WindowState.Minimized;
            };
            ToggleStateControl btnMax = (ToggleStateControl)baseWindowTemplate.FindName("btnMax", this);
            btnMax.Click += delegate
            {
                if (btnMax.IsChecked == true)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;

                }
                //NativeMethods.SetWindowRectanle(this);
            };

            Button btnClose = (Button)baseWindowTemplate.FindName("btnClose", this);
            btnClose.Click += delegate
            {
                this.Close();
            };

            Border borderTitle = (Border)baseWindowTemplate.FindName("borderTitle", this);

            borderTitle.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (WindowState != WindowState.Maximized)
                        this.DragMove();
                    //NativeMethods.SetWindowRectanle(this);
                }
            };
            borderTitle.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
            {
                if (e.ClickCount >= 2)
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;
                        btnMax.IsChecked = false;
                    }
                    else
                    {
                        WindowState = WindowState.Maximized;
                        btnMax.IsChecked = true;
                    }
                    //NativeMethods.SetWindowRectanle(this);
                }
            };
            #endregion
        }
        Rect NormalRect { get; set; }
        private void SetWorkArea()
        {
            Rect rc = SystemParameters.WorkArea;
            int width, height;
            Windows32Operation.WindowsLocation location = Windows32Operation.GetWindowsBarLocation(out width, out height);
            switch (location)
            {
                case Windows32Operation.WindowsLocation.bottom:
                    this.Left = 0;//设置位置
                    this.Top = 0;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
                case Windows32Operation.WindowsLocation.left:
                    this.Left = width;
                    this.Top = 0;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
                case Windows32Operation.WindowsLocation.right:
                    this.Left = 0;//设置位置
                    this.Top = 0;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
                case Windows32Operation.WindowsLocation.top:
                    this.Left = 0;
                    this.Top = height;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
            }
            this.MaxHeight = rc.Height;
            this.MaxWidth = rc.Width;
        }

        #region 窗体拖拽缩放

        private HwndSource hwndSource;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public void BaseWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.ResizeMode == ResizeMode.NoResize)
                return;
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            if (fe == null)
                return;
            string strTemp = fe.Name;
            switch (strTemp)
            {
                case "topRec":
                    this.Cursor = Cursors.SizeNS;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61443), IntPtr.Zero);
                    break;
                case "botRec":
                    this.Cursor = Cursors.SizeNS;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61446), IntPtr.Zero);
                    break;
                case "rightRec":
                    this.Cursor = Cursors.SizeWE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61442), IntPtr.Zero);
                    break;
                case "leftRec":
                    this.Cursor = Cursors.SizeWE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61441), IntPtr.Zero);
                    break;
                case "nwseUpRec":
                    this.Cursor = Cursors.SizeNWSE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61444), IntPtr.Zero);
                    break;
                case "nwseDownRec":
                    this.Cursor = Cursors.SizeNWSE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61448), IntPtr.Zero);
                    break;
                case "neswDownRec":
                    this.Cursor = Cursors.SizeNESW;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61447), IntPtr.Zero);
                    break;
                case "neswUpRec":
                    this.Cursor = Cursors.SizeNESW;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61445), IntPtr.Zero);
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;

            }
            NormalRect = new Rect(this.Left, this.Top, this.Width, this.Height);
            //NativeMethods.SetWindowRectanle(this);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
