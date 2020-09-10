using System;

namespace Careful.Core.Mvvm.VMBase
{
    public interface ICloseWindow
    {
        Action CloseAction { get; set; }
    }
}