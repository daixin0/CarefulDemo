using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Core.PropertyValidation
{
    public class ValidationAttributeRule : ValidationRule
    {
        public ValidationObject ValidationModel { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = null;
            
            ValidationFactory.VerifivationProrperty(ValidationModel, value, new Action<ValidationResult>(p =>
            {
                result = p;
            }));
            if (!result.IsValid)
                return result;
            if (ValidationModel.CustomValidationAction != null)
                result = new ValidationResult(ValidationModel.CustomValidationAction(value), null);
            return result;
        }
    }
}
