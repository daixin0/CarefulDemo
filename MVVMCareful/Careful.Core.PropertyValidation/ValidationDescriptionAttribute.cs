using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Effects;

namespace Careful.Core.PropertyValidation
{
    
    public class ValidationTypes
    {
        public DataValidationType DataValidationType { get; set; }

        public ValidationDescriptionType ValidationDescriptionType { get; set; }

        public DigitValidationType DigitValidationType { get; set; }
    }
    public class ValidationDescriptionAttribute : DescriptionAttribute
    {
        public ValidationDescriptionAttribute(string description) : base(description)
        {
            ValidationObject = new ValidationTypes[] { 
                new ValidationTypes() {
                    ValidationDescriptionType = ValidationDescriptionType.NotNull,
                    DataValidationType = DataValidationType.None,
                    DigitValidationType = DigitValidationType.None
            } };
        }

        public ValidationTypes[] ValidationObject { get; set; }

        public IValidationData ValidationData { get; set; }

    }
    public interface IValidationData
    {

    }
    public class DataValidation : IValidationData
    {
        public decimal Data1 { get; set; }
        public decimal Data2 { get; set; }
    }
    public class DigitValidation : IValidationData
    {
        public int DightNumber1 { get; set; }
        public int DightNumber2 { get; set; }
    }
}
