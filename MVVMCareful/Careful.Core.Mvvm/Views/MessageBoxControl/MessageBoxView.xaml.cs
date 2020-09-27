using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Careful.Core.Mvvm.Views.MessageBoxControl
{
    /// <summary>
    /// MessageBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxView : UserControl
    {
        public MessageBoxView()
        {
            InitializeComponent();
        }
        private Window _Window;

        private void OnShow(object sender, MessageBoxEventArgs e)
        {
            _Window = new MessageBoxWindow();
            _Window.Content = this;
            //_Window.SizeToContent = SizeToContent.WidthAndHeight;
            _Window.ResizeMode = ResizeMode.NoResize;
            _Window.WindowStyle = WindowStyle.None;
            _Window.Title = e.Caption;
            _Window.AllowsTransparency = true;
            _Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _Window.ShowInTaskbar = false;
            _Window.Topmost = true;
            _Window.ShowDialog();
        }

        private void OnClose(object sender, MessageBoxEventArgs e)
        {
            _Window.Close();
        }
    }
}
