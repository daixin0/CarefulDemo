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
    public class ValidationFactory
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
            object[] att = propertyInfo.GetCustomAttributes(typeof(ValidationDescriptionAttribute), true);
            if (att.Length > 0)
            {
                foreach (ValidationDescriptionAttribute verificationAttribute in att)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    foreach (var item in verificationAttribute.ValidationObject)
                    {
                        switch (item.ValidationDescriptionType)
                        {
                            case ValidationDescriptionType.NotNull:
                                if (propertyType == typeof(string))
                                {
                                    if (obj == null)
                                    {
                                        resultAction(new ValidationResult(false, verificationAttribute.Description));
                                        return false;
                                    }

                                    string value = obj.ToString();
                                    if (string.IsNullOrWhiteSpace(value))
                                    {
                                        resultAction(new ValidationResult(false, verificationAttribute.Description));
                                        return false;
                                    }
                                }
                                break;
                            case ValidationDescriptionType.Number:
                                DataValidation dataValidation = verificationAttribute.ValidationData as DataValidation;
                                switch (item.DataValidationType)
                                {
                                    case DataValidationType.LessThan:
                                        break;
                                    case DataValidationType.LessThanEqual:
                                        break;
                                    case DataValidationType.GreaterThan:
                                        break;
                                    case DataValidationType.GreaterThanEqual:
                                       
                                        bool verifyResult = ValidationHelper.GreaterThanEqual(propertyType, obj, dataValidation.Data1);
                                        if (!verifyResult)
                                        {
                                            resultAction(new ValidationResult(false, verificationAttribute.Description));
                                            return false;
                                        }
                                        break;
                                    case DataValidationType.Section:
                                        bool sectionResult = ValidationHelper.Section(obj, dataValidation.Data1, dataValidation.Data2);
                                        if (!sectionResult)
                                        {
                                            resultAction(new ValidationResult(false, verificationAttribute.Description));
                                            return false;
                                        }
                                        break;
                                    case DataValidationType.None:
                                        break;
                                }
                                break;
                            case ValidationDescriptionType.Digit:
                                DigitValidation digitValidation = verificationAttribute.ValidationData as DigitValidation;
                                switch (item.DigitValidationType)
                                {
                                    case DigitValidationType.LessThan:
                                        break;
                                    case DigitValidationType.LessThanEqual:
                                        bool numberResult = ValidationHelper.LessThanEqualDigit(obj, digitValidation.DightNumber1);
                                        if (!numberResult)
                                        {
                                            resultAction(new ValidationResult(false, verificationAttribute.Description));
                                            return false;
                                        }
                                        break;
                                    case DigitValidationType.GreaterThan:
                                        bool passWordresult = ValidationHelper.GreaterThanDigit(obj, digitValidation.DightNumber1);
                                        if (!passWordresult)
                                        {
                                            resultAction(new ValidationResult(false, verificationAttribute.Description));
                                            return false;
                                        }
                                        break;
                                    case DigitValidationType.GreaterThanEqual:
                                        break;
                                    case DigitValidationType.Section:
                                        break;
                                    case DigitValidationType.None:
                                        break;
                                }
                                break;
                            case ValidationDescriptionType.Letter:
                                bool letterResult = ValidationHelper.Letter(obj);
                                if (!letterResult)
                                {
                                    resultAction(new ValidationResult(false, verificationAttribute.Description));
                                    return false;
                                }
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

        
    }
}
