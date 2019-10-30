namespace WPFCommonLib.Base
{
    /// <summary>
    /// 需要接受数据的 ViewModel 基类
    /// </summary>
    public abstract class ViewModelBaseRoot : ViewModelBase
    {
        /// <summary>
        /// 从其它窗口传递过来的数据
        /// </summary>
        public abstract object Data { get; set; }
    }

    /// <summary>
    /// 需要接受数据的 ViewModel 基类（指定了数据的类型）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public abstract class ViewModelBaseSub<T> : ViewModelBaseRoot
    {
        /// <summary>
        /// 从其它窗口传递过来的数据
        /// </summary>
        public override object Data
        {
            get { return this.InternalData; }
            set { this.InternalData = (T)value; }
        }

        /// <summary>
        /// 从其它窗口传递过来的数据（在 VM 中使用此属性）
        /// </summary>
        protected abstract T InternalData { get; set; }
    }
}