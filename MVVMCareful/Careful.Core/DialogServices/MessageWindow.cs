using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Careful.Core.DialogServices
{
    public static class MessageWindow
    {
        public static IMessageView MessageView { get; set; }

        public static void Show(string title,string message,MessageBoxType messageBoxType,PathGeometry logoPath=null)
        {
            MessageView.Title = title;
            MessageView.Message = message;
            MessageView.MessageBoxType = messageBoxType;
            MessageView.LogoPath = logoPath;
            if (MessageView is Window view)
            {
                view.Owner = WindowOperation.GetCurrentActivatedWindow();
                view.Show();
            }
        }
        public static bool? ShowDialog(string title, string message, MessageBoxType messageBoxType, PathGeometry logoPath = null)
        {
            MessageView.Title = title;
            MessageView.Message = message;
            MessageView.MessageBoxType = messageBoxType;
            MessageView.LogoPath = logoPath;
            if (MessageView is Window view)
            {
                view.Owner = WindowOperation.GetCurrentActivatedWindow();
                return view.ShowDialog();
            }
            else
            {
                return null;
            }
        }
    }
}
