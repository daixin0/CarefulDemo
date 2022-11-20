using Careful.Controls.DesignerCanvasControl.ActivityItem;
using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Test.Models
{
    public class ActivityInfoModel:NotifyPropertyChanged
    {
        private string _activityName;

        /// <summary>
        /// Get or set ActivityName value
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { Set(ref _activityName, value); }
        }
        private Type _activityType;

        /// <summary>
        /// Get or set Activity value
        /// </summary>
        public Type ActivityType
        {
            get { return _activityType; }
            set { Set(ref _activityType, value); }
        }

    }
}
