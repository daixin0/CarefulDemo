using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Careful.Core.CommonConverter
{
    public class StringAsBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            if (value.ToString() == "1")
            {
                return true;
            }
            if (value.ToString() == "0")
            {
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            bool result = (bool)value;
            if (result)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
    public class StringAsBool2Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            if (value.ToString() == "1")
            {
                return false;
            }
            if (value.ToString() == "0")
            {
                return true;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "1";
            bool result = (bool)value;
            if (result)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }
    }
}
