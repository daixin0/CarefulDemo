using Careful.Controls.WindowBaseControl;
using Careful.Core;
using Careful.Core.DialogServices;
using System.Windows;
using System.Windows.Media;

namespace Careful.Controls.MessageBoxControl
{
    /// <summary>
    /// MessageBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : BaseWindow, IMessageView
    {
        public MessageBoxWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }


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

        public PathGeometry LogoPath
        {
            get { return (PathGeometry)GetValue(LogoPathProperty); }
            set { SetValue(LogoPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogoPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoPathProperty =
            DependencyProperty.Register("LogoPath", typeof(PathGeometry), typeof(MessageBoxWindow));



        private void MessageBox_Close(object sender, RoutedEventArgs e)
        {
            if (mb.MessageResult == ButtonResult.Yes || mb.MessageResult == ButtonResult.OK)
                this.DialogResult = true;
            else if (mb.MessageResult == ButtonResult.No || mb.MessageResult == ButtonResult.Cancel)
                this.DialogResult = false;
            else
                this.DialogResult = null;
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
