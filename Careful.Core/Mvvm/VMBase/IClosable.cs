using System;

namespace Careful.Core.Mvvm.VMBase
{
    public interface IClosable
    {
        Action CloseWindow { get; set; }
    }
}