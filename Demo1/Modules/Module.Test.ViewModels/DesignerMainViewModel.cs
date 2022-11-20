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
    public class DesignerMainViewModel: ViewModelBase
    {
        private ObservableCollection<FlowItemModel> _flowItemModels=new ObservableCollection<FlowItemModel>();

        /// <summary>
        /// Get or set FlowItemModels value
        /// </summary>
        public ObservableCollection<FlowItemModel> FlowItemModels
        {
            get { return _flowItemModels; }
            set { Set(ref _flowItemModels, value); }
        }
        private FlowItemModel _flowItemSelected;

        /// <summary>
        /// Get or set FlowItemSelected value
        /// </summary>
        public FlowItemModel FlowItemSelected
        {
            get { return _flowItemSelected; }
            set { Set(ref _flowItemSelected, value); }
        }
        private FlowItemModel _currentFlowItem = new FlowItemModel();

        /// <summary>
        /// Get or set CurrentFlowItem value
        /// </summary>
        public FlowItemModel CurrentFlowItem
        {
            get { return _currentFlowItem; }
            set { Set(ref _currentFlowItem, value); }
        }

        public ICommand NewFlowCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    FlowItemModel flowItemModel = new FlowItemModel();
                    flowItemModel.FlowName = "流程1";
                    FlowItemModels.Add(flowItemModel);
                    FlowItemSelected = flowItemModel;

                    CurrentFlowItem = new FlowItemModel();
                    CurrentFlowItem.FlowID = flowItemModel.FlowID;
                    CurrentFlowItem.FlowName = flowItemModel.FlowName;
                });
            }
        }
        public ICommand OpenFlowCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    if (p == null)
                    {
                        if (FlowItemModels.Count(s => s.FlowID == CurrentFlowItem.FlowID) > 0)
                            return;
                        if (string.IsNullOrWhiteSpace(CurrentFlowItem.FlowFilePath))
                            return;
                        FlowItemModel flowItemModel = new FlowItemModel();
                        flowItemModel.FlowID = CurrentFlowItem.FlowID;
                        flowItemModel.FlowName = CurrentFlowItem.FlowName;
                        flowItemModel.FlowFilePath = CurrentFlowItem.FlowFilePath;
                        flowItemModel.IsSave = true;
                        FlowItemModels.Add(flowItemModel);
                        FlowItemSelected = flowItemModel;
                    }
                    else
                    {
                        CurrentFlowItem.FlowID = FlowItemSelected.FlowID;
                        CurrentFlowItem.FlowName = FlowItemSelected.FlowName;
                        CurrentFlowItem.FlowFilePath = FlowItemSelected.FlowFilePath;
                    }
                    
                });
            }
        }

        public ICommand SaveFlowCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    if (FlowItemSelected == null)
                        return;
                    FlowItemSelected.IsSave = true;
                    FlowItemSelected.FlowFilePath = CurrentFlowItem.FlowFilePath;
                });
            }
        }




    }
}
