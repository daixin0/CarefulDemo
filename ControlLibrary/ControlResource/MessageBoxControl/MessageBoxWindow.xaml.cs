using Careful.Controls.WindowBaseControl;
using Careful.Core;
using Careful.Core.DialogServices;
using System;
using System.Windows;
using System.Windows.Media;

namespace Careful.Controls.MessageBoxControl
{
    public partial class MessageBoxWindow : BaseWindow, IMessageView
    {
        public MessageBoxWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            WindowType = typeof(MessageBoxWindow);
        }
        public Type WindowType { get; set; }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxWindow));



        public MessageBoxType MessageBoxType
        {
            get { return (MessageBoxType)GetValue(MessageBoxTypeProperty); }
            set { SetValue(MessageBoxTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageBoxType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageBoxTypeProperty =
            DependencyProperty.Register("MessageBoxType", typeof(MessageBoxType), typeof(MessageBoxWindow));



        public MessageButtonType MessageButtonType
        {
            get { return (MessageButtonType)GetValue(MessageButtonTypeProperty); }
            set { SetValue(MessageButtonTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageButtonType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageButtonTypeProperty =
            DependencyProperty.Register("MessageButtonType", typeof(MessageButtonType), typeof(MessageBoxWindow), new PropertyMetadata(MessageButtonType.Default));


        public string DetermineText
        {
            get { return (string)GetValue(DetermineTextProperty); }
            set { SetValue(DetermineTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetermineText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetermineTextProperty =
            DependencyProperty.Register("DetermineText", typeof(string), typeof(MessageBoxWindow));



        public string CancelText
        {
            get { return (string)GetValue(CancelTextProperty); }
            set { SetValue(CancelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelTextProperty =
            DependencyProperty.Register("CancelText", typeof(string), typeof(MessageBoxWindow));


        public Geometry LogoPath
        {
            get { return (Geometry)GetValue(LogoPathProperty); }
            set { SetValue(LogoPathProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LogoPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoPathProperty =
            DependencyProperty.Register("LogoPath", typeof(Geometry), typeof(MessageBoxWindow));



        public double Button1Width
        {
            get { return (double)GetValue(Button1WidthProperty); }
            set { SetValue(Button1WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Button1Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Button1WidthProperty =
            DependencyProperty.Register("Button1Width", typeof(double), typeof(MessageBoxWindow));



        public double Button2Width
        {
            get { return (double)GetValue(Button2WidthProperty); }
            set { SetValue(Button2WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Button2Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Button2WidthProperty =
            DependencyProperty.Register("Button2Width", typeof(double), typeof(MessageBoxWindow));



        private void MessageBox_Close(object sender, RoutedEventArgs e)
        {
            if (mb.MessageResult == ButtonResult.Yes || mb.MessageResult == ButtonResult.OK)
                this.DialogResult = true;
            else if (mb.MessageResult == ButtonResult.No || mb.MessageResult == ButtonResult.Cancel)
                this.DialogResult = false;
            else
                this.DialogResult = null;
        }

    }
}
