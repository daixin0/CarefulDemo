﻿using Careful.Core.Ioc;
using Careful.Core.Services;
using Careful.Core.Tool;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Careful.Core.Mvvm.VMBase
{
    public class ViewModelDialog : ViewModelBase, IDialogService
    {
        private readonly IContainerExtension _containerExtension;
        public ViewModelDialog(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }
        protected virtual Window CreateDialogWindow(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _containerExtension.Resolve<Window>();
            else
                return _containerExtension.Resolve<Window>(name);
        }
        protected virtual void DialogWindowEvents(Window dialogWindow, Action<IDialogResult> callback)
        {
            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (o) =>
            {
                if (dialogWindow.DataContext is IDialogResult dialogResult)
                {
                    dialogResult.Result = o.Result;
                    dialogResult.Parameters = o.Parameters;
                }
                dialogWindow.Close();
            };

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                dialogWindow.Loaded -= loadedHandler;
                if (dialogWindow.DataContext is IDialogAware dialogAware)
                    dialogAware.RequestClose += requestCloseHandler;
            };
            dialogWindow.Loaded += loadedHandler;

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (dialogWindow.DataContext is IDialogAware dialogAware)
                    if (!dialogAware.CanCloseDialog())
                        e.Cancel = true;
            };
            dialogWindow.Closing += closingHandler;

            EventHandler closedHandler = null;
            closedHandler = (o, e) =>
            {
                dialogWindow.Closed -= closedHandler;
                dialogWindow.Closing -= closingHandler;
                if (dialogWindow.DataContext is IDialogAware dialogAware)
                {
                    dialogAware.RequestClose -= requestCloseHandler;

                    dialogAware.OnDialogClosed();

                }
                if (dialogWindow.DataContext is IDialogResult dialogResult)
                {
                    if (dialogResult == null)
                        dialogResult = new DialogResult();
                    callback?.Invoke(dialogResult);
                }
                dialogWindow.DataContext = null;
                dialogWindow.Content = null;
            };
            dialogWindow.Closed += closedHandler;
        }

        private void CommonShow(string name, IDialogParameters parameters, Action<IDialogResult> callback, bool isModel)
        {
            Window window = null;
            try
            {
                window = CreateDialogWindow(name);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Cannot get a window", ex);
            }
            DialogWindowEvents(window, callback);

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(window.DataContext, d => d.OnDialogOpened(parameters));

            if (isModel)
            {
                window.Owner = WindowOperation.GetCurrentActivatedWindow();

                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            CommonShow(name, parameters, callback, false);
        }
        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            CommonShow(name, parameters, callback, true);
        }
        public bool Confirm(string message, string title = "询问")
        {
            throw new NotImplementedException();
        }

        public void Confirm(IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowLog(string title, string log)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message, string title = "提示")
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowMessageDialog(string message, string title = "提示")
        {
            throw new NotImplementedException();
        }

        public void ShowMessageDialog(IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

    }
}