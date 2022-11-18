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
    public class InputAttribute: Attribute
    {
        public InputAttribute(string name)
        {
            _name = name;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }

        private bool _necessary = true;

        public bool Necessary
        {
            get { return _necessary; }
            set { _necessary = value; }
        }
    }
}
