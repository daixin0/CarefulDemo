using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Effects;

namespace Careful.Core.PropertyValidation
{
    
    public class ValidationDescriptionAttribute : Attribute
    {
        public ValidationDescriptionAttribute()
        {
            ValidationDescriptionType = ValidationDescriptionType.None;
            DataValidationType = DataValidationType.None;
            DigitValidationType = DigitValidationType.None;

        }
        public ValidationDescriptionAttribute(double data1):base()
        {
            Data1 = data1;
        }
        public ValidationDescriptionAttribute(double data1,double data2) : base()
        {
            Data1 = data1;
            Data2 = data2;
        }
        public string Description { get; set; }
        public DataValidationType DataValidationType { get; set; }

        public ValidationDescriptionType ValidationDescriptionType { get; set; }

        public DigitValidationType DigitValidationType { get; set; }
        public double Data1 { get; set; }
        public double Data2 { get; set; }
        public int DightNumber1 { get; set; }
        public int DightNumber2 { get; set; }


    }
}
