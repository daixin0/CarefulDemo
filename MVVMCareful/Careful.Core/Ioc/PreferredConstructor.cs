using System;

namespace Careful.Core.Ioc
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class PreferredConstructorAttribute : Attribute
    {
    }
}