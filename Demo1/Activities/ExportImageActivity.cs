using Careful.Controls.DesignerCanvasControl.ActivityItem;
using Careful.Controls.DesignerCanvasControl.ConnectorControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activities
{
    [ActivityVision("ExportImageActivity")]
    public class ExportImageActivity: Activity
    {
        [InputAttribute("输入图像")]
        public Image InputImage { get; set; }

        public string SavePath { get; set; }

        public override void RunProc()
        {
            InputImage.Save(SavePath);
        }

        public override bool ValidateData()
        {
            if (InputImage == null)
                return false;
            if (string.IsNullOrWhiteSpace(SavePath))
                return false;
            return true;
        }
    }
}
