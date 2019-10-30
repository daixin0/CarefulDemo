using System;

namespace WPFCommonLib.Base
{
    public class OpenWindowInfo
    {
        public bool IsModal { get; set; }
        public object Parameter { get; set; }
        public Type WindowType { get; set; }
    }
}