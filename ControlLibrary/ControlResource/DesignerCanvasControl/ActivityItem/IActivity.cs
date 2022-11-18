using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.DesignerCanvasControl.ActivityItem
{
    public interface IActivity
    {
        string ID { get; set; }
        string ActivityName { get; set; }
        string Description { get; set; }
        ActivityState ActivityState { get; set; }
        bool HasInputs();
        bool HasOutputs();
        bool ValidateData();
        bool ValidateConnection();
        void Execute();
    }
    public enum ActivityState
    {
        Waiting,
        Runnable,
        Running,
        Finished,
        Abort
    }
    public enum ActivityCommand
    {
        Validate,
        Execute
    }
}
