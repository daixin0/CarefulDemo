using Careful.Controls.DesignerCanvasControl.ActivityItem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Activities
{
    [ActivityVision("ImportImageActivity")]
    public class ImportImageActivity:Activity
    {
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(ImportImageActivity));

        [OutputAttribute("输出图像")]
        public Image OutputImage { get; set; }

        public override void RunProc()
        {
            Image image = new Bitmap(ImagePath);
            OutputImage = image;
        }
        public override bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(ImagePath))
                return false;
            if (!File.Exists(ImagePath))
                return false;
            return true;
        }
    }
}
