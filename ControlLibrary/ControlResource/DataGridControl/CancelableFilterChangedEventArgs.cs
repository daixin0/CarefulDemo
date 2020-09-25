using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Careful.Controls.DataGridControl
{
    public class CancelableFilterChangedEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public Predicate<object> NewFilter { get; set; }
        public CancelableFilterChangedEventArgs(Predicate<object> p)
        {
            NewFilter = p;
        }
    }
}
