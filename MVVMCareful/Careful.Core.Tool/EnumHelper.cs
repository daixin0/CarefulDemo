using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Tool
{
    public class EnumHelper
    {
        public static List<string> GetEnumPropertyName(Type enumType)
        {
            List<string> enumPropertyList = new List<string>();
            var values = Enum.GetValues(enumType);
            for (int i = 0; i < values.Length; i++)
            {
                var v = values.GetValue(i);
                enumPropertyList.Add(v.ToString());
            }
            return enumPropertyList;
        }
    }
}
