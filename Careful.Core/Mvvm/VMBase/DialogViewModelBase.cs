using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.Mvvm.VMBase
{
    public class DialogViewModelBase : ViewModelBase
    {
        public Action Closed { get; set; }
        public void Close()
        {
            Closed?.Invoke();
        }
    }
}
