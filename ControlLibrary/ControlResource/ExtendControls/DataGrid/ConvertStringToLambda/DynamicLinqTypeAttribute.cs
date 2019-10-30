using System;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    /// <summary>
    /// Indicates to Dynamic Linq to consider the Type as a valid dynamic linq type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public sealed class DynamicLinqTypeAttribute : Attribute
    {
    }
}