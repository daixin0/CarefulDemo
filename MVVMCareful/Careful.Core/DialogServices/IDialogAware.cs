using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.DialogServices
{
    public interface IDialogAware
    {
        void OnDialogOpened(IDialogParameters parameters);
        void OnDialogClosed();
        bool CanCloseDialog();

        event Action<IDialogResult> RequestClose;
    }
}
