using Careful.Controls.DesignerCanvasControl.ActivityItem;
using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Careful.Controls.WorkFlowControl
{
    public class WorkFlow:NotifyPropertyChanged
    {
        private string _workFlowName;

        /// <summary>
        /// Get or set WorkFlowName value
        /// </summary>
        public string WorkFlowName
        {
            get { return _workFlowName; }
            set { Set(ref _workFlowName, value); }
        }
        private string _description;

        /// <summary>
        /// Get or set Description value
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
        private ObservableCollection<Activity> _activities;

        /// <summary>
        /// 所有活动
        /// </summary>
        public ObservableCollection<Activity> Activities
        {
            get { return _activities; }
        }
        private ObservableCollection<Activity> _positive;

        /// <summary>
        /// 通过检查或者执行成功的活动
        /// </summary>
        public ObservableCollection<Activity> Positive
        {
            get { return _positive; }
            set { _positive = value; }
        }
        private ObservableCollection<Activity> _ambiguity = new ObservableCollection<Activity>();

        /// <summary>
        /// 因为条件不充分没有执行或者检查的活动
        /// </summary>
        public ObservableCollection<Activity> Ambiguity
        {
            get { return _ambiguity; }
        }

        private ObservableCollection<Activity> _negative = new ObservableCollection<Activity>();

        /// <summary>
        /// 执行不成功或者检查不通过的活动
        /// </summary>
        public ObservableCollection<Activity> Negative
        {
            get { return _negative; }
        }

        public void Execute()
        {

        }
    }
}
