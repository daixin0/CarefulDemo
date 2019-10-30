using JetBrains.Annotations;
using System;
using System.ComponentModel;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    internal static class TypeConverterFactory
    {
        /// <summary>
        /// Returns a type converter for the specified type.
        /// </summary>
        /// <param name="type">The System.Type of the target component.</param>
        /// <returns>A System.ComponentModel.TypeConverter for the specified type.</returns>
        public static TypeConverter GetConverter([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));
            return TypeDescriptor.GetConverter(type);
        }
    }
}