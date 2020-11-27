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
using Careful.Core;
using Careful.Core.Mvvm.Views;
using Careful.Core.Ioc;
using Careful.Controls.ToggleExtendControl;

namespace Careful.Controls.WindowBaseControl
{
    public class BaseWindow : Window, INotifyPropertyChanged, IView
    {

        public BaseWindow()
        {
            Application application = CarefulIoc.Default.GetInstance<Application>();
            this.Style = (Style)application.Resources["BaseWindowStyle"];
            this.DataContext = this;
            this.StateChanged += BaseWindow_StateChanged;
        }
        //static BaseWindow()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));

        //    FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
        //    metadata.Inherits = true;
        //    metadata.DefaultValue = 2;
        //    metadata.AffectsMeasure = true;
        //    metadata.PropertyChangedCallback += (d, e) => { };

        //    metadata = new FrameworkPropertyMetadata();
        //    metadata.Inherits = true;
        //    metadata.DefaultValue = 40;
        //    metadata.AffectsMeasure = true;
        //    metadata.PropertyChangedCallback += (d, e) => { };
        //}

        //public BaseWindow() : base()
        //{
        //    this.DataContext = this;
        //    this.StateChanged += BaseWindow_StateChanged;
        //}
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


        public event RoutedEventHandler BeforeClose
        {
            add { AddHandler(BeforeCloseEvent, value); }
            remove { RemoveHandler(BeforeCloseEvent, value); }
        }

        public static readonly RoutedEvent BeforeCloseEvent = EventManager.RegisterRoutedEvent(
            "BeforeClose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaseWindow));




        public bool IsResize
        {
            get { return (bool)GetValue(IsResizeProperty); }
            set { SetValue(IsResizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsResize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsResizeProperty =
            DependencyProperty.Register("IsResize", typeof(bool), typeof(BaseWindow));



        public double TitleHeight
        {
            get { return (double)GetValue(TitleHeightProperty); }
            set { SetValue(TitleHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleHeightProperty =
            DependencyProperty.Register("TitleHeight", typeof(double), typeof(BaseWindow), new PropertyMetadata(36.0));



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
            #region 窗体操作

            Button minBtn = (Button)Template.FindName("btnMin", this);
            minBtn.Click += delegate
            {
                WindowState = WindowState.Minimized;
            };
            ToggleExtend btnMax = (ToggleExtend)Template.FindName("btnMax", this);
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

            Button btnClose = (Button)Template.FindName("btnClose", this);
            btnClose.Click += delegate
            {
                RoutedEventArgs routedEventArgs = new RoutedEventArgs(BaseWindow.BeforeCloseEvent, this);
                this.RaiseEvent(routedEventArgs);
                this.Close();
            };

            Border borderTitle = (Border)Template.FindName("borderTitle", this);

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
                if (IsMaxButton)
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

        public bool IsSingle
        {
            get { return (bool)GetValue(IsSingleProperty); }
            set { SetValue(IsSingleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSingle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSingleProperty =
            DependencyProperty.Register("IsSingle", typeof(bool), typeof(BaseWindow), new PropertyMetadata(false));


        private void SetWorkArea()
        {
            Rect rc = SystemParameters.WorkArea;
            int width, height;
            WindowOperation.WindowsLocation location = WindowOperation.GetWindowsBarLocation(out width, out height);
            switch (location)
            {
                case WindowOperation.WindowsLocation.bottom:
                    this.Left = 0;//设置位置
                    this.Top = 0;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
                case WindowOperation.WindowsLocation.left:
                    this.Left = width;
                    this.Top = 0;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
                case WindowOperation.WindowsLocation.right:
                    this.Left = 0;//设置位置
                    this.Top = 0;
                    this.Width = rc.Width;
                    this.Height = rc.Height;
                    break;
                case WindowOperation.WindowsLocation.top:
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
