using Careful.Controls.DesignerCanvasControl.ActivityItem;
using Careful.Controls.DesignerCanvasControl.ConnectorControl;
using Careful.Core.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Activities
{
    [ActivityVision("InvertImageActivity")]
    public class InvertImageActivity: Activity
    {
        [OutputAttribute("输出图像")]
        public Image OutputImage { get; set; }

        [InputAttribute("输入图像")]
        public Image InputImage { get; set; }

        public Image Invert(Image bitmapImage)
        {
            Bitmap btimap = new Bitmap(bitmapImage);
            for (int y = 0; y < btimap.Height; y++)
            {
                for (int x = 0; x < btimap.Width; x++)
                {
                    var pixel = btimap.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    btimap.SetPixel(x, y, newColor);
                }
            }
            Image image = btimap;
            return image;
        }
		public override void RunProc()
        {
            OutputImage = Invert(InputImage);
        }

        public override bool ValidateData()
        {
            if (InputImage == null)
                return false;
            return true;
        }
    }
}
