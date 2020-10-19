using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Careful.Core.PropertyValidation
{
    public enum DataValidationType
    {
        None,
        LessThan,
        LessThanEqual,
        GreaterThan,
        GreaterThanEqual,
        Equal,
        Section
    }
    public enum DigitValidationType
    {
        None,
        LessThan,
        LessThanEqual,
        GreaterThan,
        GreaterThanEqual,
        Equal,
        Section
    }
    public enum ValidationDescriptionType
    {
        NotNull,
        Number,
        Digit,
        Letter
    }
}
