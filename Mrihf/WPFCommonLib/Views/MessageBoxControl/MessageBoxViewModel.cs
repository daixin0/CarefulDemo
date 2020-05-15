using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFCommonLib.Command;

namespace WPFCommonLib.Views.MessageBoxControl
{
    public class MessageBoxViewModel : NotifyPropertyChanged
    {
        #region Fields

        private Path _Image;
        private string _Message;
        private string _Caption;
        private string _Ok;
        private string _Yes;
        private string _No;
        private string _Cancel;
        private bool _OkVisibility;
        private bool _YesVisibility;
        private bool _NoVisibility;
        private bool _CancelVisibility;
        private bool _IsOkDefault;
        private bool _IsYesDefault;
        private bool _IsNoDefault;
        private bool _IsCancelDefault;

        private const string _DefaultOk = "确定";
        private const string _DefaultCancel = "取消";
        private const string _DefaultYes = "是";
        private const string _DefaultNo = "否";

        public event EventHandler<MessageBoxEventArgs> ShowMessageBoxEventHandler;
        public event EventHandler<MessageBoxEventArgs> CloseMessageBoxEventHandler;
        private MessageBoxButton _MessageBoxButton;

        #endregion

        #region Constructors

        public MessageBoxViewModel()
        {
            Ok = _DefaultOk;
            Yes = _DefaultYes;
            No = _DefaultNo;
            Cancel = _DefaultCancel;

            OkVisibility = true;
            YesVisibility = false;
            NoVisibility = false;
            CancelVisibility = false;

            IsOkDefault = true;
            IsYesDefault = false;
            IsNoDefault = false;
            IsCancelDefault = false;

            OkCommand = new RelayCommand(param => OnOkClicked());
            YesCommand = new RelayCommand(param => OnYesClicked());
            NoCommand = new RelayCommand(param => OnNoClicked());
            CancelCommand = new RelayCommand(param => OnCancelClicked());

            Caption = string.Empty;
            Result = MessageBoxResult.Cancel;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The message box image.</value>
        public Path Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    RaisePropertyChanged("Image");
                }
            }
        }

        /// <summary>
        /// Gets or sets the message box message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    RaisePropertyChanged("Message");
                }
            }
        }

        /// <summary>
        /// Gets or sets the message box caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                if (_Caption != value)
                {
                    _Caption = value;
                    RaisePropertyChanged("Caption");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Ok button content.
        /// </summary>
        /// <value>The Ok button content.</value>
        public string Ok
        {
            get
            {
                return _Ok;
            }
            set
            {
                _Ok = value;
                RaisePropertyChanged("Ok");
            }
        }

        /// <summary>
        /// Gets or sets the Yes button content.
        /// </summary>
        /// <value>The Yes button content.</value>
        public string Yes
        {
            get
            {
                return _Yes;
            }
            set
            {
                _Yes = value;
                RaisePropertyChanged("Yes");
            }
        }

        /// <summary>
        /// Gets or sets the No button content.
        /// </summary>
        /// <value>The No button content.</value>
        public string No
        {
            get
            {
                return _No;
            }
            set
            {
                _No = value;
                RaisePropertyChanged("No");
            }
        }

        /// <summary>
        /// Gets or sets the Cancel button content.
        /// </summary>
        /// <value>The Cancel button content.</value>
        public string Cancel
        {
            get
            {
                return _Cancel;
            }
            set
            {
                _Cancel = value;
                RaisePropertyChanged("Cancel");
            }
        }
        private SolidColorBrush _backgroundTitle;

        public SolidColorBrush BackgroundTitle
        {
            get
            {
                return _backgroundTitle;
            }
            set
            {
                _backgroundTitle = value;
                RaisePropertyChanged("BackgroundTitle");
            }
        }

        public MessageBoxButton MessageBoxButton
        {
            get
            {
                return _MessageBoxButton;
            }
            set
            {
                _MessageBoxButton = value;

                switch (value)
                {
                    case MessageBoxButton.Ok:
                        OkVisibility = true;
                        IsOkDefault = true;
                        YesVisibility = false;
                        IsYesDefault = false;
                        NoVisibility = false;
                        IsNoDefault = false;
                        CancelVisibility = false;
                        IsCancelDefault = false;
                        Ok = _DefaultOk;
                        BackgroundTitle = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF33BB01"));
                        break;
                    case MessageBoxButton.OkCancel:
                        OkVisibility = true;
                        IsOkDefault = true;
                        YesVisibility = false;
                        IsYesDefault = false;
                        NoVisibility = false;
                        IsNoDefault = false;
                        CancelVisibility = true;
                        IsCancelDefault = false;
                        Ok = _DefaultOk;
                        Cancel = _DefaultCancel;
                        break;
                    case MessageBoxButton.YesNo:
                        OkVisibility = false;
                        IsOkDefault = false;
                        YesVisibility = true;
                        IsYesDefault = true;
                        NoVisibility = true;
                        IsNoDefault = false;
                        CancelVisibility = false;
                        IsCancelDefault = false;
                        Yes = _DefaultYes;
                        No = _DefaultNo;
                        BackgroundTitle = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008FFF"));
                        break;
                    case MessageBoxButton.YesNoCancel:
                        OkVisibility = false;
                        IsOkDefault = false;
                        YesVisibility = true;
                        IsYesDefault = true;
                        NoVisibility = true;
                        IsNoDefault = false;
                        CancelVisibility = true;
                        IsCancelDefault = false;
                        Yes = _DefaultYes;
                        No = _DefaultNo;
                        Cancel = _DefaultCancel;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Gets or sets the visibility of the Ok button.
        /// </summary>
        /// <value>The visibility of the Ok button.</value>
        public bool OkVisibility
        {
            get
            {
                return _OkVisibility;
            }
            set
            {
                _OkVisibility = value;
                RaisePropertyChanged("OkVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the Yes button.
        /// </summary>
        /// <value>The visibility of the Yes button.</value>
        public bool YesVisibility
        {
            get
            {
                return _YesVisibility;
            }
            set
            {
                _YesVisibility = value;
                RaisePropertyChanged("YesVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the No button.
        /// </summary>
        /// <value>The visibility of the No button.</value>
        public bool NoVisibility
        {
            get
            {
                return _NoVisibility;
            }
            set
            {
                _NoVisibility = value;
                RaisePropertyChanged("NoVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the Cancel button.
        /// </summary>
        /// <value>The visibility of the Cancel button.</value>
        public bool CancelVisibility
        {
            get
            {
                return _CancelVisibility;
            }
            set
            {
                _CancelVisibility = value;
                RaisePropertyChanged("CancelVisibility");
            }
        }

        public bool IsOkDefault
        {
            get
            {
                return _IsOkDefault;
            }
            set
            {
                _IsOkDefault = value;
                RaisePropertyChanged("IsOkDefault");
            }
        }

        public bool IsYesDefault
        {
            get
            {
                return _IsYesDefault;
            }
            set
            {
                _IsYesDefault = value;
                RaisePropertyChanged("IsYesDefault");
            }
        }

        public bool IsNoDefault
        {
            get
            {
                return _IsNoDefault;
            }
            set
            {
                _IsNoDefault = value;
                RaisePropertyChanged("IsNoDefault");
            }
        }

        public bool IsCancelDefault
        {
            get
            {
                return _IsCancelDefault;
            }
            set
            {
                _IsCancelDefault = value;
                RaisePropertyChanged("IsCancelDefault");
            }
        }

        public ICommand OkCommand
        {
            get;
            private set;
        }

        public ICommand YesCommand
        {
            get;
            private set;
        }

        public ICommand NoCommand
        {
            get;
            private set;
        }

        public ICommand CancelCommand
        {
            get;
            private set;
        }

        public MessageBoxResult Result
        {
            get;
            private set;
        }

        #endregion

        #region Methods


        private void OnOkClicked()
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void OnYesClicked()
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void OnNoClicked()
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void OnCancelClicked()
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void Close()
        {
            if (CloseMessageBoxEventHandler != null)
            {
                CloseMessageBoxEventHandler(this, new MessageBoxEventArgs());
            }
        }

        public void SetDefaultButton(MessageBoxResult defaultButton)
        {
            switch (defaultButton)
            {
                case MessageBoxResult.OK:
                    IsOkDefault = true;
                    IsYesDefault = false;
                    IsNoDefault = false;
                    IsCancelDefault = false;
                    break;
                case MessageBoxResult.Cancel:
                    IsOkDefault = false;
                    IsYesDefault = false;
                    IsNoDefault = false;
                    IsCancelDefault = true;
                    break;
                case MessageBoxResult.Yes:
                    IsOkDefault = false;
                    IsYesDefault = true;
                    IsNoDefault = false;
                    IsCancelDefault = false;
                    break;
                case MessageBoxResult.No:
                    IsOkDefault = false;
                    IsYesDefault = false;
                    IsNoDefault = true;
                    IsCancelDefault = false;
                    break;
                default:
                    break;
            }
        }

        public MessageBoxResult Show()
        {
            if (ShowMessageBoxEventHandler != null)
            {
                ShowMessageBoxEventHandler(this, new MessageBoxEventArgs() { Caption = Caption });
            }
            return Result;
        }

        public MessageBoxResult Show(string message)
        {
            Message = message;
            return Show();
        }

        public MessageBoxResult Show(string message, string caption)
        {
            Message = message;
            Caption = caption;
            return Show();
        }

        public MessageBoxResult Show(string message, Path image)
        {
            Message = message;
            Image = image;
            return Show();
        }

        public MessageBoxResult Show(string message, MessageBoxButton button)
        {
            Message = message;
            MessageBoxButton = button;
            return Show();
        }

        public MessageBoxResult Show(string message, string caption, MessageBoxButton button)
        {
            Message = message;
            Caption = caption;
            MessageBoxButton = button;
            return Show();
        }

        public MessageBoxResult Show(string message, string caption, Path image)
        {
            Message = message;
            Caption = caption;
            Image = image;
            return Show();
        }

        public MessageBoxResult Show(string message, Path image, MessageBoxButton button)
        {
            Message = message;
            Image = image;
            MessageBoxButton = button;
            return Show();
        }

        public MessageBoxResult Show(string message, string caption, MessageBoxButton button, Path image)
        {
            Message = message;
            Caption = caption;
            MessageBoxButton = button;
            Image = image;
            return Show();
        }

        #endregion
    }
}
