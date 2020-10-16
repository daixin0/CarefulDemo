using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Careful.Core.DialogServices
{
    public interface IMessageView
    {
        string Title { get; set; }
        string Message { get; set; }

        MessageButtonType MessageButtonType { get; set; }

        string DetermineText { get; set; }
        string CancelText { get; set; }

        double Button1Width { get; set; }
        double Button2Width { get; set; }

        MessageBoxType MessageBoxType { get; set; }

        Geometry LogoPath { get; set; }

        Type WindowType { get; set; }
    }
}
