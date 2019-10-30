using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlResource.ExtendControlStyle.DataGrid
{
    public class FilterChangedEventArgs : EventArgs
    {
        public Predicate<object> Filter { get; set; }

        public FilterChangedEventArgs(Predicate<object> p)
        {
            Filter = p;
        }
    }
}
