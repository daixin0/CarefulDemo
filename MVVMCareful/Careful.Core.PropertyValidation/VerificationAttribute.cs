using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Careful.Core.PropertyValidation
{
    public class VerificationAttribute : Attribute
    {
        public VerificationType VerificationType { get; set; }
        public string PropertyDescription { get; set; }

    }
}
