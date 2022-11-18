using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.DesignerCanvasControl.ActivityItem
{
    [AttributeUsage(AttributeTargets.Property,
       AllowMultiple = false,
       Inherited = true)]
    public class OutputAttribute : Attribute
    {
        public OutputAttribute(string name)
        {
            _name = name;
        }

        string _name;

        public string Name
        {
            get { return _name; }
        }
    }
}
