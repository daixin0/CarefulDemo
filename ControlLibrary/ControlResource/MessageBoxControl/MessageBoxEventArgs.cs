using Careful.Core.DialogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Controls.MessageBoxControl
{
    public class MessageBoxEventArgs : RoutedEventArgs
    {
        public MessageBoxEventArgs(RoutedEvent routedEvent, ButtonResult result)
            : base(routedEvent, result)
        {
            Result = result;
        }
        public ButtonResult Result { get; set; }
    }
}
