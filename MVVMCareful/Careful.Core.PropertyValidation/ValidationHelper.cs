using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Careful.Core.PropertyValidation
{
    public class ValidationHelper
    {
        public static bool VerifyPositivePhoneNumber(Type propertyType, object obj)
        {
            string phoneNumber = obj.ToString();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            if (phoneNumber.Length > 11) { return false; }
            Regex regex = new Regex("^1[34578]\\d{9}$");
            return regex.IsMatch(phoneNumber);
        }
        public static bool GreaterThanEqual(Type propertyType, object obj, double data1)
        {
            if (obj is null)
                return false;
            string value = obj.ToString();
            if (value.EndsWith("."))
                return false;
            if (propertyType == typeof(decimal))
                return double.TryParse(value, out double number) && number >= data1;
            else if (propertyType == typeof(int))
                return int.TryParse(value, out int number) && number >= data1;
            else if (propertyType == typeof(double))
                return double.TryParse(value, out double number) && number >= double.Parse(data1.ToString());
            else
                return false;
        }
        public static bool GreaterThan(Type propertyType, object obj, double data1)
        {
            if (obj is null)
                return false;
            string value = obj.ToString();
            if (value.EndsWith("."))
                return false;
            if (propertyType == typeof(decimal))
                return double.TryParse(value, out double number) && number > data1;
            else if (propertyType == typeof(int))
                return int.TryParse(value, out int number) && number > data1;
            else if (propertyType == typeof(double))
                return double.TryParse(value, out double number) && number > double.Parse(data1.ToString());
            else
                return false;
        }
        public static bool LessThan(Type propertyType, object obj, double data1)
        {
            if (obj is null)
                return false;
            string value = obj.ToString();
            if (value.EndsWith("."))
                return false;
            if (propertyType == typeof(decimal))
                return double.TryParse(value, out double number) && number < data1;
            else if (propertyType == typeof(int))
                return int.TryParse(value, out int number) && number < data1;
            else if (propertyType == typeof(double))
                return double.TryParse(value, out double number) && number < double.Parse(data1.ToString());
            else
                return false;
        }
        public static bool LessThanEqual(Type propertyType, object obj, double data1)
        {
            if (obj is null)
                return false;
            string value = obj.ToString();
            if (value.EndsWith("."))
                return false;
            if (propertyType == typeof(decimal))
                return double.TryParse(value, out double number) && number <= data1;
            else if (propertyType == typeof(int))
                return int.TryParse(value, out int number) && number <= data1;
            else if (propertyType == typeof(double))
                return double.TryParse(value, out double number) && number <= double.Parse(data1.ToString());
            else
                return false;
        }
        public static bool Section(object obj, double number1, double number2)
        {
            if (obj == null) return true;
            double value = 0.0;
            if (double.TryParse(obj?.ToString(), out value))
            {
                if (value <= number2 && value >= number1)
                    return true;
                else return false;
            }
            else
                return false;
            
        }
        public static bool GreaterThanDigit(object obj, int number)
        {
            if (obj == null) return true;
            if (obj.ToString().Length < number)
            {
                return false;
            }
            return true;
        }
        public static bool LessThanEqualDigit(object obj, int number)
        {
            if (obj == null) return true;
            if (obj.ToString().Length > number)
            {
                return false;
            }
            return true;
        }

        public static bool Letter(object obj)
        {
            if (obj == null) return true;
            if (Regex.IsMatch(obj.ToString(), @"(?i)^[a-z]+$"))
                return true;
            else return false;
        }
    }
}
