using System;
using System.Collections.Generic;

namespace WPFCommonLib.Misc
{
    public class ValueDescriptionAttribute : Attribute
    {
        public ValueDescriptionAttribute()
        {
            Values = new Dictionary<string, string> { };
        }

        public ValueDescriptionAttribute(string[] parts) : this()
        {
            foreach (var part in parts)
            {
                var keyValue = part.Split(',');
                if (keyValue.Length >= 2)
                {
                    if (!Values.ContainsKey(keyValue[0]))
                    {
                        Values.Add(keyValue[0], keyValue[1]);
                    }
                }
            }
        }

        public Dictionary<string, string> Values { get; set; }
    }
}