using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Careful.Core.DialogServices
{
    public class MessageWindow
    {
        public MessageWindow(IMessageView messageView)
        {
            MessageView = messageView;
        }
        private IMessageView MessageView { get; set; }

        public void Show(string title, string message, MessageBoxType messageBoxType, Geometry logoPath = null, MessageButtonType messageButtonType = MessageButtonType.Default, string determineText = "是", string cancelText = "否", double button1Width = 60, double button2Width = 60)
        {
            if (MessageView is Window)
            {
                Window view = Activator.CreateInstance(MessageView.WindowType) as Window;
                MessageView = view as IMessageView;
                MessageView.Title = title;
                MessageView.Message = message;
                MessageView.MessageBoxType = messageBoxType;
                MessageView.LogoPath = logoPath;
                MessageView.MessageButtonType = messageButtonType;
                MessageView.DetermineText = determineText;
                MessageView.CancelText = cancelText;
                MessageView.Button1Width = button1Width;
                MessageView.Button2Width = button2Width;
                (MessageView as Window).Owner = WindowOperation.GetCurrentActivatedWindow();
                (MessageView as Window).Show();
            }
        }
        public bool? ShowDialog(string title, string message, MessageBoxType messageBoxType, Geometry logoPath = null, MessageButtonType messageButtonType = MessageButtonType.Default, string determineText = "是", string cancelText = "否", double button1Width = 60, double button2Width = 60)
        {
            if (MessageView is Window)
            {
                Window view = Activator.CreateInstance(MessageView.WindowType) as Window;
                MessageView = view as IMessageView;
                MessageView.Title = title;
                MessageView.Message = message;
                MessageView.MessageBoxType = messageBoxType;
                MessageView.LogoPath = logoPath;
                MessageView.MessageButtonType = messageButtonType;
                MessageView.DetermineText = determineText;
                MessageView.CancelText = cancelText;
                MessageView.Button1Width = button1Width;
                MessageView.Button2Width = button2Width;
                (MessageView as Window).Owner = WindowOperation.GetCurrentActivatedWindow();
                return (MessageView as Window).ShowDialog();
            }
            else
            {
                return null;
            }
        }
    }
}
