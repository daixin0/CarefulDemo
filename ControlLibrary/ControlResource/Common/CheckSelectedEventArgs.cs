using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.Common
{
    public class CheckSelectedEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public bool IsCheckAll { get; set; }
        public CheckSelectedEventArgs(bool IsCheckAll)
        {
            this.IsCheckAll = IsCheckAll;
        }
    }
}
