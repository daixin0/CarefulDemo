using CommonLib.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFCommonLib.Helpers;
using WPFCommonLib.Views;

namespace WPFCommonLib.Services
{
    public class FrameWindowService : IFrameWindowService
    {
        private List<Action> _tearDownActions = new List<Action>();

        public FrameWindowService()
        {
            FrameWindow = SimpleIoc.Default.GetInstance<FrameWindow>();
            FrameWindow.Closed += FrameWindow_Closed;
            FrameWindow.Closing += FrameWindow_Closing;
        }

        private FrameWindow FrameWindow { get; set; }

        public void ChangeContent(string contentFullqualifiedName)
        {
            var controlType = ApplicationHelper.GetTargetType(contentFullqualifiedName);
            if (controlType == null)
            {
                return;
            }

            var control = Activator.CreateInstance(controlType) as UserControl;
            if (control == null)
            {
                return;
            }

            FrameWindow.WindowContainer.Content = control;
        }

        public void DequeueTearDownAction(Action action)
        {
            var targetAction = _tearDownActions.FirstOrDefault(m => m.Method.Name == action.Method.Name && m.Target == action.Target);
            if (targetAction != null)
            {
                _tearDownActions.Remove(targetAction);
            }
        }

        public void HideBusyInfo()
        {
            FrameWindow.WindowContainer.WaitingMessage = string.Empty;
            FrameWindow.WindowContainer.ShowProgressIndicator = false;
        }

        public void QueueTearDownAction(Action action)
        {
            if (!_tearDownActions.Any(a => a.Method.Name == action.Method.Name && a.Target == action.Target))
            {
                _tearDownActions.Add(action);
            }
        }

        public void SetClosingCheckFunction(Func<bool> windowClosingConfirm)
        {
            _windowClosingConfirm = windowClosingConfirm;
        }

        public void ShowBusyInfo(string message)
        {
            FrameWindow.WindowContainer.WaitingMessage = message;

            if (!FrameWindow.WindowContainer.ShowProgressIndicator)
            {
                FrameWindow.WindowContainer.ShowProgressIndicator = true;
            }
        }

        public void UpdateTitle(string title)
        {
            FrameWindow.Title = title;
        }

        private void FrameWindow_Closed(object sender, EventArgs e)
        {
            foreach (var action in _tearDownActions)
            {
                action.Invoke();
            }
            _tearDownActions.Clear();
        }

        private Func<bool> _windowClosingConfirm;

        private void FrameWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_windowClosingConfirm == null)
            {
                return;
            }

            e.Cancel = !_windowClosingConfirm();
        }
    }
}