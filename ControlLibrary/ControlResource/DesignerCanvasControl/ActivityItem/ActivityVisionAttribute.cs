using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Careful.Controls.DesignerCanvasControl.ActivityItem
{
    public class ActivityVisionAttribute : Attribute
    {
        public ActivityVisionAttribute(string name)
        {
            _name = name;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 在toolbox中显示的图片
        /// </summary>
        public Image ToolBoxImage { get; set; }

        /// <summary>
        /// 在设计器中显示的图片
        /// </summary>
        public Image DesignerImage { get; set; }
    }
}
