using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Careful.Core.Mvvm.Views.MessageBoxControl
{
    public static class MessageBoxImage
    {

        static ResourceDictionary pathStyle = new ResourceDictionary() { Source = new Uri("WPFCommonLib;component/Views/MessageBoxControl/MessageBoxPath.xaml", UriKind.Relative) };

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
    }
    public enum MessageBoxImageType
    {
        Information,
        Question
    }
}
