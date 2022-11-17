using Careful.Controls.Common;
using Careful.Controls.Common.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Controls.ActivityControl
{
    public class Activity : Control, IActivity
    {

        public string ActivityName
        {
            get { return (string)GetValue(ActivityNameProperty); }
            set { SetValue(ActivityNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActivityName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivityNameProperty =
            DependencyProperty.Register("ActivityName", typeof(string), typeof(Activity));



        public IOutputData OutputData
        {
            get { return (IOutputData)GetValue(OutputDataProperty); }
            set { SetValue(OutputDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputDataProperty =
            DependencyProperty.Register("OutputData", typeof(IOutputData), typeof(Activity));



        public IInputData InputData
        {
            get { return (IInputData)GetValue(InputDataProperty); }
            set { SetValue(InputDataProperty, value); }
        }


        // Using a DependencyProperty as the backing store for InputData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputDataProperty =
            DependencyProperty.Register("InputData", typeof(IInputData), typeof(Activity));


        public void Execute()
        {
            
        }
    }
}
