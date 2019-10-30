using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCommonLib.Misc
{

    public class ColumnDataTypeAttribute : Attribute
    {
        public Type DataType { get; set; }
    }
}
