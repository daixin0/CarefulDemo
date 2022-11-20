using Careful.Core.Mvvm.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Test.Models
{
    public class FlowItemModel: NotifyPropertyChanged
    {
        private string _flowName;

        /// <summary>
        /// Get or set FlowName value
        /// </summary>
        public string FlowName
        {
            get { return _flowName; }
            set { Set(ref _flowName, value); }
        }
        private string _flowID = Guid.NewGuid().ToString();

        /// <summary>
        /// Get or set FlowID value
        /// </summary>
        public string FlowID
        {
            get { return _flowID; }
            set { Set(ref _flowID, value); }
        }

        private string _flowFilePath;

        /// <summary>
        /// Get or set FlowFilePath value
        /// </summary>
        public string FlowFilePath
        {
            get { return _flowFilePath; }
            set { Set(ref _flowFilePath, value); }
        }
        private bool _isSave;

        /// <summary>
        /// Get or set IsSave value
        /// </summary>
        public bool IsSave
        {
            get { return _isSave; }
            set { Set(ref _isSave, value); }
        }

    }
}
