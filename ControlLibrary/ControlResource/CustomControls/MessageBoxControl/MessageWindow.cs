using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ControlResource.CustomControlsStyle.MessageBoxControl
{
    public class MessageWindow
    {
        public static MessageBoxResult ShowMessageBoxWindow(string content, string title)
        {
            MessageBoxViewModel vm = (MessageBoxViewModel)new MessageBoxView().FindResource("ViewModel");
            vm.IsYesDefault = true;
            return vm.Show(content, title, MessageBoxButton.OkCancel, MessageBoxImage.Information);
        }
        public static MessageBoxResult ShowMessageBoxWindow(string content, string title,MessageBoxButton ButtonType)
        {
            MessageBoxViewModel vm = (MessageBoxViewModel)new MessageBoxView().FindResource("ViewModel");
            vm.IsYesDefault = true;
            return vm.Show(content, title, ButtonType,MessageBoxImage.Information);
        }
        public static MessageBoxResult ShowMessageBoxWindow(string content, string title, MessageBoxButton ButtonType, MessageBoxImageType ImageIco)
        {
            MessageBoxViewModel vm = (MessageBoxViewModel)new MessageBoxView().FindResource("ViewModel");
            vm.IsYesDefault = true;
            switch (ImageIco)
            {
                case MessageBoxImageType.Information:
                    return vm.Show(content, title, ButtonType, MessageBoxImage.Information);
                case MessageBoxImageType.Question:
                    return vm.Show(content, title, ButtonType, MessageBoxImage.Question);
            }
            return vm.Show(content, title, ButtonType, MessageBoxImage.Information);
        }
    }
}
