using Careful.Controls.DesignerCanvasControl.ActivityItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.DesignerCanvasControl.ConnectorControl
{
    public class PortEnd
    {
        public Activity Activity { get; set; }
        public string Port { get; set; }

        public object GetPortValue()
        {
            Type type = this.Activity.GetType();
            PropertyInfo ppt = type.GetProperty(this.Port);
            return ppt.GetValue(this.Activity, null);
        }

        public void SetPortValue(object v)
        {
            Type type = this.Activity.GetType();
            PropertyInfo ppt = type.GetProperty(this.Port);
            ppt.SetValue(this.Activity, v, null);
        }
    }
}
