using Careful.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.Services
{
    public class DialogParameters : ParametersBase, IDialogParameters
    {
        public DialogParameters() : base() { }

        public DialogParameters(string query) : base(query) { }
    }
}
