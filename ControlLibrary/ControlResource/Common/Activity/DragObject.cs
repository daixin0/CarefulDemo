using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Controls.Common.Activity
{
    public class DragObject
    {
        // Xaml string that represents the serialized content
        public object DesignControl { get; set; }
        public object DesignData { get; set; }

        // Defines width and height of the DesignerItem
        // when this DragObject is dropped on the DesignerCanvas
        public Size? DesiredSize { get; set; }
    }
}
