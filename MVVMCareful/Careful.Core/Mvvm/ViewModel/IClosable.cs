using System;

namespace Careful.Core.Mvvm.ViewModel
{
    public interface IClosable
    {
        Action CloseWindow { get; set; }
    }
}