using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlResource.CustomControlsStyle.MessageBoxControl
{
    public static class MessageBoxImage
    {

        static ResourceDictionary pathStyle = new ResourceDictionary() { Source = new Uri("ControlResource;component/CustomControlsStyle/MessageBoxControl/MessageBoxPath.xaml", UriKind.Relative) };

        private static Path _information;

        public static Path Information
        {
            get {
                _information = new Path();
                _information = (Path)pathStyle["pathInformation"];

                return _information; }
        }

        private static Path _question;

        public static Path Question
        {
            get
            {
                _question = new Path();
                _question = (Path)pathStyle["pathQuestion"];

                return _question;
            }
        }
        
        //public static BitmapImage Information = new BitmapImage(new System.Uri("pack://application:,,,/MessageBoxShow;component/Images/zz.png"));
        //public static BitmapImage Question = new BitmapImage(new System.Uri("pack://application:,,,/MessageBoxShow;component/Images/Question.bmp"));
        //public static BitmapImage Warning = new BitmapImage(new System.Uri("pack://application:,,,/MessageBoxShow;component/Images/Warning.bmp"));
        //public static BitmapImage Error = new BitmapImage(new System.Uri("pack://application:,,,/MessageBoxShow;component/Images/cuo.png"));
    }
    public enum MessageBoxImageType
    {
        Information,
        Question
    }
}
