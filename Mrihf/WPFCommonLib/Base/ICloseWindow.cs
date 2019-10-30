using System;

namespace WPFCommonLib.Base
{
    public interface ICloseWindow
    {
        Action CloseAction { get; set; }
    }
}