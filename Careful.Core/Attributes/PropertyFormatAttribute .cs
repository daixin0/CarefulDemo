using Careful.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Careful.Core.Attributes
{
    public class PropertyFormatOutputAttribute : Attribute
    {
        public PropertyFormatOutputAttribute(string format, params string[] propertyNames)
        {
            FormatString = format;
            PropertyNames = propertyNames.HasItems() ? propertyNames : new string[] { };
        }

        public string FormatString { get; set; }

        public string[] PropertyNames { get; set; }
    }
}