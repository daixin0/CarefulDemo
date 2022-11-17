using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.Common.Activity
{
    public interface IActivity
    {
        string ActivityName { get; set; }
        IOutputData OutputData { get; set; }
        IInputData InputData { get; set; }

        void Execute();
    }
}
