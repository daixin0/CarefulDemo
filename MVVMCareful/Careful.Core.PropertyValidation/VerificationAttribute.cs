using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Careful.Core.PropertyValidation
{
    public enum VerificationType
    {
        [Description("不能为空")]
        NotNull,
        [Description("不能为小于0")]
        PositiveNumber,
        [Description("手机号")]
        PhoneNumber,
        [Description("不能少于6位")]
        Password
    }
    public class VerificationAttribute : Attribute
    {
        public VerificationType VerificationType { get; set; }
        public string PropertyDescription { get; set; }

    }
}
