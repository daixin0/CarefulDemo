using System;
using System.Collections.Generic;
using WPFCommonLib.Extensions;

namespace WPFCommonLib.Misc
{
    /// <summary>
    /// 格式化输出，对给定的属性名根据指定的格式输出
    /// </summary>
    public class PropertyFormatOutputAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="format"></param>
        /// <param name="propertyNames"></param>
        public PropertyFormatOutputAttribute(string format, params string[] propertyNames)
        {
            FormatString = format;
            PropertyNames = propertyNames.HasItems() ? propertyNames : new string[] { };
        }

        /// <summary>
        /// 格式化字符串，与 string.Format 中使用的是格式一致
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// 属性名数组
        /// </summary>
        public string[] PropertyNames { get; set; }
    }
}