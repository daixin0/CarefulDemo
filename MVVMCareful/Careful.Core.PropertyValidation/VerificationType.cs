using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Careful.Core.PropertyValidation
{
    public enum VerificationType
    {
        [Description("不能为空")]
        NotNull,
        [Description("仅支持整数，且不能为负数")]
        PositiveNumber,
        [Description("手机号")]
        PhoneNumber,
        [Description("最多5位")]
        LessThanNumber,
        [Description("必须大于6位")]
        GreaterThanNumber,
        [Description("仅限英文字母")]
        Letter
    }
}
