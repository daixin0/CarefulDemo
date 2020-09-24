using Careful.Core.Mvvm;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;

namespace Careful.Controls.PagingControl
{
    public class PagingCommandItem : NotifyPropertyChanged, ICommand
    {
        protected internal PagingCommandItem(PagingScroller zapScroller, int index, string pageNumber = "",int buttonIndex=0,bool isFirst=false)
        {
            //Contract.Requires<ArgumentNullException>(zapScroller != null);
            //Contract.Requires<ArgumentOutOfRangeException>(index >= 0);

            m_zapScroller = zapScroller;

            m_zapScroller.CurrentItemChanged += delegate (object sender, RoutedPropertyChangedEventArgs<int> e)
            {
                OnCanExecuteChanged(EventArgs.Empty);
            };

            IsFirst = isFirst;
            PageNumber = pageNumber;
            m_index = index;
            ButtonIndex = buttonIndex;
            //m_content = m_zapScroller.Items[m_index];
        }

        public object Content
        {
            get { return m_content; ; }
            protected internal set
            {
                if (m_content != value)
                {
                    m_content = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Content"));
                }
            }
        }

        public int Index { get { return m_index; } }

        /// <remarks>
        ///     For public use. Most people don't like zero-base indices.
        /// </remarks>
        public int Number { get { return m_index + 1; } }

        private string _pageNumber;

        public string PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = value;
            }
        }

        private bool _isFirst;

        public bool IsFirst
        {
            get
            {
                return _isFirst;
            }
            set
            {
                _isFirst = value;
            }
        }

        private int _buttonIndex;

        public int ButtonIndex
        {
            get
            {
                return _buttonIndex;
            }
            set
            {
                _buttonIndex = value;
            }
        }
        public bool CanExecute
        {
            get
            {
                return (m_index != m_zapScroller.CurrentItemIndex);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute;
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("CanExecute"));

            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void MakeCurrent()
        {
            if (PageNumber == "...")
            {
                if (IsFirst)
                {
                    if (m_zapScroller.CurrentItemIndex - 5 < 0)
                    {
                        m_index = 0;
                    }
                    else
                    {
                        m_index = m_zapScroller.CurrentItemIndex - 5;
                    }
                }
                else
                {
                    if (m_zapScroller.CurrentItemIndex + 5 > m_zapScroller.PageNumber - 1)
                    {
                        m_index = m_zapScroller.PageNumber - 1;
                    }
                    else
                    {
                        m_index = m_zapScroller.CurrentItemIndex + 5;
                    }
                }
                
            }

            m_zapScroller.CurrentItemIndex = m_index;

            m_zapScroller.SelectedIndex = ButtonIndex;
        }

        void ICommand.Execute(object parameter)
        {
            MakeCurrent();
        }

        //public override string ToString()
        //{
        //    string output = (Content == null) ? "*null*" : Content.ToString();

        //    return string.Format("ZapCommandItem - Index: {0}, Content: {1}", m_index, output);
        //}

        #region Implementation

        private object m_content;

        private int m_index;
        private readonly PagingScroller m_zapScroller;

        #endregion
    }
}
