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

        MessageBoxType MessageBoxType { get; set; }

        PathGeometry LogoPath { get; set; }
    }
}
