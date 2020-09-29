using Careful.Core.Logs;
using System;

namespace Careful.Core.DialogServices
{
    public interface IDialogWindow
    {
        void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback);
        void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback);
    }
    public interface IDialogService
    {
        bool Confirm(string message, string title = "询问");
        void Confirm(IDialogParameters parameters, Action<IDialogResult> callback);
        void ShowLog(string title, string log, LogLevel logLevel, Priority priority = Priority.None);
        void ShowMessage(string message, string title = "提示");
        void ShowMessageDialog(string message, string title = "提示");
        void ShowMessage(IDialogParameters parameters, Action<IDialogResult> callback);
        void ShowMessageDialog(IDialogParameters parameters, Action<IDialogResult> callback);
        
    }
}