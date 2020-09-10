using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.DialogServices
{
    public class DialogResult : IDialogResult
    {
        public IDialogParameters Parameters { get; set; } = new DialogParameters();

        public ButtonResult Result { get; set; } = ButtonResult.None;

        public DialogResult() { }

        public DialogResult(ButtonResult result)
        {
            Result = result;
        }

        public DialogResult(ButtonResult result, IDialogParameters parameters)
        {
            Result = result;
            Parameters = parameters;
        }
    }
}
