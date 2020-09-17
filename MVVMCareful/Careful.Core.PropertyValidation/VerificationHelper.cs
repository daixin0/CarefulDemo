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
                object[] att = pro.GetCustomAttributes(typeof(VerificationAttribute), true);
                if (att.Length > 0)
                {
                    VerificationAttribute verificationAttribute = att[0] as VerificationAttribute;
                    Type proType = pro.PropertyType;
                    switch (verificationAttribute.VerificationType)
                    {
                        case VerificationType.NotNull:
                            if (proType == typeof(string))
                            {
                                string value = pro.GetValue(obj)?.ToString();
                                if (string.IsNullOrWhiteSpace(value))
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                                    return false;
                                }
                            }
                            break;
                        case VerificationType.PositiveNumber:
                            string valNumber = pro.GetValue(obj)?.ToString();
                            bool verifyResult = VerifyPositiveNumber(proType, valNumber);
                            if (!verifyResult)
                            {
                                resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                                return false;
                            }
                            break;
                        case VerificationType.Password:
                            string passWord = pro.GetValue(obj)?.ToString();
                            bool passWordresult = VerifyPositivePassWord(proType, passWord);
                            if (!passWordresult)
                            {
                                resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                                return false;
                            }
                            break;
                        default:
                            break;

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
            object[] att = propertyInfo.GetCustomAttributes(typeof(VerificationAttribute), true);
            if (att.Length > 0)
            {
                VerificationAttribute verificationAttribute = att[0] as VerificationAttribute;
                Type propertyType = propertyInfo.PropertyType;
                switch (verificationAttribute.VerificationType)
                {
                    case VerificationType.NotNull:
                        if (propertyType == typeof(string))
                        {
                            if (obj == null)
                            {
                                resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                                return false;
                            }
                            string value = obj.ToString();
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                                return false;
                            }
                        }
                        break;
                    case VerificationType.PositiveNumber:
                        bool verifyResult = VerifyPositiveNumber(propertyType, obj);
                        if (!verifyResult)
                        {
                            resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                            return false;
                        }
                        break;
                    case VerificationType.PhoneNumber:
                        bool rest = VerifyPositivePhoneNumber(propertyType, obj);
                        if (!rest)
                        {
                            resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                            return false;
                        }
                        break;
                    case VerificationType.Password:
                    
                        bool passWordresult = VerifyPositivePassWord(propertyType, obj);
                        if (!passWordresult)
                        {
                            resultAction(new ValidationResult(false, verificationAttribute.PropertyDescription + verificationAttribute.VerificationType.GetDescription()));
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            resultAction(new ValidationResult(true, null));
            return true;
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

        private static bool VerifyPositivePassWord(Type propertyType, object obj) 
        {
            if (obj == null) return false;
            string passWord = obj.ToString();
            if (string.IsNullOrEmpty(passWord))
            {
                return false;
            }
            if (passWord.Length < 6) { return false; }
            return true;
        }

    }
}
