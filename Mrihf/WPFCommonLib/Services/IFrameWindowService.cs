using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using WPFCommonLib.Views.WindowContainerControl;

namespace WPFCommonLib.Services
{
    public interface IFrameWindowService : IProgressIndicator, IWindow, IWindowClosingConfirm
    {
        /// <summary>
        /// 改变容器中的内容
        /// </summary>
        /// <param name="contentFullQualifiedName">控件的全名</param>
        void ChangeContent(string contentFullQualifiedName);
    }

    public interface IContainerService
    {
        WindowContainer ContainerContent { get; set; }
        /// <summary>
        /// 改变容器中的内容
        /// </summary>
        /// <param name="contentFullQualifiedName">控件的全名</param>
        void ChangeContent(string contentFullQualifiedName, bool isAnimation = true, object obj = null);
        void ChangeContent(string contentFullQualifiedName, WindowContainer containerContent);
        void ChangeContent(string contentFullQualifiedName, WindowContainer containerContent, object obj = null, Func<bool> closeFunc = null);
    }

    public interface IProgressIndicator
    {
        /// <summary>
        /// 隐藏进度信息
        /// </summary>
        void HideBusyInfo();

        /// <summary>
        /// 显示进度信息
        /// </summary>
        /// <param name="message"></param>
        void ShowBusyInfo(string message);
    }

    public interface IWindow
    {
        /// <summary>
        /// 从操作列表中移除，这个操作列表是窗口关闭时要执行的
        /// </summary>
        /// <param name="action"></param>
        void DequeueTearDownAction(Action action);

        /// <summary>
        /// 当窗口要关闭时，要执行的操作
        /// </summary>
        /// <param name="action"></param>
        void QueueTearDownAction(Action action);

        /// <summary>
        /// 更新窗口标题
        /// </summary>
        /// <param name="title"></param>
        void UpdateTitle(string title);
    }

    public interface IWindowClosingConfirm
    {
        void SetClosingCheckFunction(Func<bool> windowClosingConfirm);
    }
}