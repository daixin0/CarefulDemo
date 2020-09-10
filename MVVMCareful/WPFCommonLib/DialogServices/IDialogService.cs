using System;
using System.Collections.Generic;

namespace WPFCommonLib.Services
{
    public enum ConfirmType
    {
        YesNo,
        OKCancel
    }

    public interface IDialogService
    {
        void Alert(string message);

        bool Confirm(string message, string title = "询问", ConfirmType type = ConfirmType.YesNo);

        void ShowException(Exception ex);

        void ShowExceptionLog(string title, string error);
        void ShowMessage(string message, string title="提示");

        bool ShowSelectFileDialog(string filter, out string filePath);
        List<string> ShowSelectMultiFileDialog(out string filePath, string filter = "");

        bool ShowSaveFileDialog(string filter, out string filePath, string defalutFileName = "");
        bool ShowSelectFolderDialog(out string folderPath, string defaultPath);
       bool ShowSelectFileDialog(string filter, out string filePath, string defalutFileName);
    }
}