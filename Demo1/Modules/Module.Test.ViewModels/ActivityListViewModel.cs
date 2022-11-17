using Careful.Core.Mvvm.Command;
using Careful.Core.Mvvm.ViewModel;
using Module.Test.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Module.Test.ViewModels
{
    public class ActivityListViewModel:ViewModelBase
    {
        private ObservableCollection<ActivityInfoModel> _activityInfoModels;

        /// <summary>
        /// Get or set ActivityInfoModels value
        /// </summary>
        public ObservableCollection<ActivityInfoModel> ActivityInfoModels
        {
            get { return _activityInfoModels; }
            set { Set(ref _activityInfoModels, value); }
        }

        public ICommand LoadCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    ActivityInfoModels = new ObservableCollection<ActivityInfoModel>();
                    ActivityInfoModels.Add(new ActivityInfoModel() { ActivityName = "测试1" });
                    ActivityInfoModels.Add(new ActivityInfoModel() { ActivityName = "测试2" });
                    ActivityInfoModels.Add(new ActivityInfoModel() { ActivityName = "测试3" });
                    ActivityInfoModels.Add(new ActivityInfoModel() { ActivityName = "测试4" });
                });
            }
        }


    }
}
