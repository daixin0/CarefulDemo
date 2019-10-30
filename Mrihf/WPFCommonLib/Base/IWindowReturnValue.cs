namespace WPFCommonLib.Base
{
    public interface IWindowReturnValue
    {
        object ReturnValue { get; set; }
    }

    public interface IWindowReturnValue<T>
    {
        T ReturnValue { get; set; }
    }

    /// <summary>
    /// 能够设置输入值和获取返回值的窗体接口
    /// </summary>
    /// <typeparam name="TPara">输入参数类型</typeparam>
    /// <typeparam name="T">返回值类型</typeparam>
    public interface IWindowInputAndReturnValue<TPara, T> : IWindowReturnValue<T>
    {
        /// <summary>
        /// 设置输入参数
        /// </summary>
        /// <param name="input"></param>
        void SetParameter(TPara input);
    }
}