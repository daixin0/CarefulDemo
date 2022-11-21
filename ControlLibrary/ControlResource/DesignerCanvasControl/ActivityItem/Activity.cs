using Careful.Controls.Common;
using Careful.Controls.DesignerCanvasControl.ConnectorControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Careful.Controls.DesignerCanvasControl.ActivityItem
{
    public class Activity : Control, IActivity
    {
        [Category("活动")]
        public string ActivityName
        {
            get { return (string)GetValue(ActivityNameProperty); }
            set { SetValue(ActivityNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActivityName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivityNameProperty =
            DependencyProperty.Register("ActivityName", typeof(string), typeof(Activity));

        [Category("活动")]
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(Activity));

        public ActivityState ActivityState
        {
            get { return (ActivityState)GetValue(ActivityStateProperty); }
            set { SetValue(ActivityStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActivityState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivityStateProperty =
            DependencyProperty.Register("ActivityState", typeof(ActivityState), typeof(Activity));

        private string _id = Guid.NewGuid().ToString();

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public event ActivityStateEventHandler StateChanged;

        public void OnStateChanged(ActivityState state, Exception e = null)
        {
            if (StateChanged != null)
                StateChanged(this, new ActivityStateEventArgs(state, e));
        }

        internal void OnStateChanged(ActivityCommand command, ActivityState state, Exception e = null)
        {
            if (StateChanged != null)
                StateChanged(this, new ActivityStateEventArgs(command, state, e));
        }

        public void Execute()
        {
            if (ActivityState == ActivityState.Running) return;
            if (ActivityState == ActivityState.Finished) return;
            if (ActivityState == ActivityState.Abort) return;

            try
            {
                ActivityState = ActivityState.Running;
                OnStateChanged(ActivityState.Running, null);

                RunProc();
                ActivityState = ActivityState.Finished;
                OnStateChanged(ActivityState.Finished, null);
            }
            catch (Exception e)
            {
                ActivityState = ActivityState.Abort;
                OnStateChanged(ActivityState.Abort, e);
            }
        }

        public virtual bool HasInputs()
        {
            return InputConnection.Count > 0;
        }

        public virtual bool HasOutputs()
        {
            return OutputConnection.Count > 0;
        }

        public virtual bool ValidateData()
        {
            return true;
        }

        public virtual bool ValidateConnection()
        {
            Type type = this.GetType();
            PropertyInfo[] ppts = type.GetProperties();
            int inputCount = 0;
            int outputCount = 0;
            foreach (var ppt in ppts)
            {
                object[] attrsInput = ppt.GetCustomAttributes(typeof(InputAttribute), true);
                if (attrsInput.Length > 0)
                {
                    inputCount++;
                }
                object[] attrsOutput = ppt.GetCustomAttributes(typeof(OutputAttribute), true);
                if (attrsOutput.Length > 0)
                {
                    outputCount++;
                }
            }
            if (inputCount != InputConnection.Count)
            {
                return false;
            }
            if (outputCount != OutputConnection.Count)
            {
                return false;
            }
            return true;
        }

        public virtual void RunProc()
        {
            
        }


        private List<Connection> _inputConnection = new List<Connection>();

        public List<Connection> InputConnection
        {
            get { return _inputConnection; }
        }
        private List<Connection> _outputConnection = new List<Connection>();
        
        public List<Connection> OutputConnection
        {
            get { return _outputConnection; }
        }

        
    }
}
