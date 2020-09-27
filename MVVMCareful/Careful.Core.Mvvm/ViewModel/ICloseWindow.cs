using System;

namespace Careful.Core.Mvvm.ViewModel
{
    public interface ICloseWindow
    {
        Action CloseAction { get; set; }
    }
}