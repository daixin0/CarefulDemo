using System;

namespace Careful.Core.Ioc
{
    /// <summary>
    /// When used with the SimpleIoc container, specifies which constructor
    /// should be used to instantiate when GetInstance is called.
    /// If there is only one constructor in the class, this attribute is
    /// not needed.
    /// </summary>
    //// [ClassInfo(typeof(SimpleIoc))]
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class PreferredConstructorAttribute : Attribute
    {
    }
}