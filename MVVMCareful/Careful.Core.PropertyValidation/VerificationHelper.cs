using Careful.Core.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Core.PropertyValidation
{
    public class VerificationHelper
    {
        public static bool VerifivationObject<T>(T obj, Action<ValidationResult> resultAction = null)
        {
            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (var pro in propertyInfos)
            {
                return VerifivationFanction(pro, pro.GetValue(obj), resultAction);
            }
            resultAction(new ValidationResult(true, null));
            return true;
        }
        private static bool VerifivationFanction(PropertyInfo propertyInfo,object obj, Action<ValidationResult> resultAction = null)
        {
            object[] att = propertyInfo.GetCustomAttributes(typeof(VerificationAttribute), true);
            if (att.Length > 0)
            {
                foreach (VerificationAttribute verificationAttribute in att)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    foreach (var item in verificationAttribute.VerificationType)
                    {
                        switch (item)
                        {
                            case VerificationType.NotNull:
                                if (propertyType == typeof(string))
                                {
                                    if (obj == null)
                                    {
                                        resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                        return false;
                                    }

                                    string value = obj.ToString();
                                    if (string.IsNullOrWhiteSpace(value))
                                    {
                                        resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                        return false;
                                    }
                                }
                                break;
                            case VerificationType.PositiveNumber:
                                bool verifyResult = VerifyPositiveNumber(propertyType, obj);
                                if (!verifyResult)
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                    return false;
                                }
                                break;
                            case VerificationType.PhoneNumber:
                                bool rest = VerifyPositivePhoneNumber(propertyType, obj);
                                if (!rest)
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                    return false;
                                }
                                break;
                            case VerificationType.GreaterThanNumber:
                                bool passWordresult = VerifyNumberGreaterThanLength(obj, 6);
                                if (!passWordresult)
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                    return false;
                                }
                                break;
                            case VerificationType.LessThanNumber:
                                bool numberResult = VerifyNumberLessThanLength(obj, 5);
                                if (!numberResult)
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                    return false;
                                }
                                break;
                            case VerificationType.Letter:
                                bool letterResult = VerifyIsLetter(obj);
                                if (!letterResult)
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + item.GetDescription()));
                                    return false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                
                
            }
            resultAction(new ValidationResult(true, null));
            return true;
        }
        public static bool VerifivationProrperty(ValidationObject validationObject, object obj, Action<ValidationResult> resultAction = null)
        {
            Type objectType = validationObject.ValidationModel.GetType();
            PropertyInfo propertyInfo = objectType.GetProperty(validationObject.PropertyName);
            return VerifivationFanction(propertyInfo, obj, resultAction);
        }
        /// <summary>
        /// 验证是否大于零
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool VerifyPositiveNumber(Type propertyType, object obj)
        {
            if (obj is null)
                return false;
            string value = obj.ToString();
            if (value.EndsWith("."))
                return false;
            if (propertyType == typeof(decimal))
                return decimal.TryParse(value, out decimal number) && number >= 0;
            else if (propertyType == typeof(int))
                return int.TryParse(value, out int number) && number >= 0;
            else if (propertyType == typeof(double))
                return double.TryParse(value, out double number) && number >= 0;
            else if (propertyType == typeof(float))
                return float.TryParse(value, out float number) && number >= 0;
            else if (propertyType == typeof(ulong))
                return ulong.TryParse(value, out ulong number) && number >= 0;
            else
                return false;
        }

        private static bool VerifyPositivePhoneNumber(Type propertyType, object obj)
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
        private static bool VerifyIsLetter(object obj)
        {
            if (obj == null) return true;
            if (Regex.IsMatch(obj.ToString(), @"(?i)^[a-z]+$"))
                return true;
            else return false;
        }
        private static bool VerifyNumberGreaterThanLength(object obj,int number)
        {
            if (obj == null) return true;
            if (obj.ToString().Length < number)
            {
                return false;
            }
            return true;
        }
        private static bool VerifyNumberLessThanLength(object obj,int number)
        {
            if (obj == null) return true;
            if (obj.ToString().Length > number)
            {
                return false;
            }
            return true;
        }
    }
}
