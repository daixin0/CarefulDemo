using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.DesignerCanvasControl.ActivityItem
{
    public delegate void ActivityStateEventHandler(object sender, ActivityStateEventArgs e);
    public class ActivityStateEventArgs : EventArgs
    {
        ActivityCommand _command;
        ActivityState _state;
        Exception _exception;

        public ActivityStateEventArgs(ActivityState state, Exception exception = null)
        {
            _state = state;
            _exception = exception;
            _command = ActivityCommand.Execute;
        }

        public ActivityStateEventArgs(ActivityCommand command, ActivityState state, Exception exception = null)
        {
            _state = state;
            _exception = exception;
            _command = command;
        }

        public ActivityCommand Command
        {
            get { return _command; }
        }

        public ActivityState State
        {
            get { return _state; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }
    }
}
