using System;

namespace WPFCommonLib.Base
{
    public interface IOpenWindow
    {
        void ShowWindow(OpenWindowInfo openWindowInfo);
    }

    public interface IOpenWindow2<TReturnValue> : IOpenWindow
    {
        void ShowWindow(OpenWindowInfo openWindowInfo, Action<TReturnValue> action);
    }

    public interface IOpenWindow2 : IOpenWindow
    {
        void ShowWindow(OpenWindowInfo openWindowInfo, Action<object> action);
    }
}