using System;

namespace WPFCommonLib.Base
{
    public interface IClosable
    {
        Action CloseWindow { get; set; }
    }
}