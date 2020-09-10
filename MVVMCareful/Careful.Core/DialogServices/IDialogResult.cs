using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.DialogServices
{
    public interface IDialogResult
    {
        IDialogParameters Parameters { get; set; }

        ButtonResult Result { get; set; }
    }
}
