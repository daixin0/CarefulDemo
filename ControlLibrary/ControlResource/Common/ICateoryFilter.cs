using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.Common
{
    public interface ICateoryFilter
    {
        string ID { get; }
        object FilterColumnValue { get; set; }
    }
}
