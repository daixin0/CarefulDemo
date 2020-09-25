using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Careful.Core.Mvvm.Command
{
   
    public abstract class DelegateCommandBase : ICommand, IActiveAware
    {
        private bool _isActive;
        private List<WeakReference> _canExecuteChangedHandlers;

        protected readonly Func<object, Task> _executeMethod;
        protected readonly Func<object, bool> _canExecuteMethod;

        
        protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Application.Current.FindResource("DelegateCommandDelegatesCannotBeNull").ToString());

            _executeMethod = (arg) => { executeMethod(arg); return Task.Delay(0); };
            _canExecuteMethod = canExecuteMethod;
        }

        
        protected DelegateCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Application.Current.FindResource("DelegateCommandDelegatesCannotBeNull").ToString());

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        protected virtual void OnCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        protected async Task Execute(object parameter)
        {
            await _executeMethod(parameter);
        }


        protected bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

       
        public virtual event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        #region IsActive
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        public virtual event EventHandler IsActiveChanged;

        protected virtual void OnIsActiveChanged()
        {
            EventHandler isActiveChangedHandler = IsActiveChanged;
            if (isActiveChangedHandler != null) isActiveChangedHandler(this, EventArgs.Empty);
        }
        #endregion
    }
}