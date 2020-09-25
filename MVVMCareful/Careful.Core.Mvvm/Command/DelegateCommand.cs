using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Careful.Core.Mvvm.Command
{
   
    public class DelegateCommand<T> : DelegateCommandBase
    {
        
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Application.Current.FindResource("DelegateCommandDelegatesCannotBeNull").ToString());

            TypeInfo genericTypeInfo = typeof(T).GetTypeInfo();

            
            if (genericTypeInfo.IsValueType)
            {
                if ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo())))
                {
                    throw new InvalidCastException(Application.Current.FindResource("DelegateCommandInvalidGenericPayloadType").ToString());
                }
            }

        }

        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod)
        {
            return new DelegateCommand<T>(executeMethod);
        }

       
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            return new DelegateCommand<T>(executeMethod, canExecuteMethod);
        }

        public virtual bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        
        public virtual async Task Execute(T parameter)
        {
            await base.Execute(parameter);
        }


        private DelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        private DelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Application.Current.FindResource("DelegateCommandDelegatesCannotBeNull").ToString());
        }

    }

    
    public class DelegateCommand : DelegateCommandBase
    {
        
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Application.Current.FindResource("DelegateCommandDelegatesCannotBeNull").ToString());
        }

       
        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod)
        {
            return new DelegateCommand(executeMethod);
        }

        
        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }

        public virtual async Task Execute()
        {
            await Execute(null);
        }

        
        public virtual bool CanExecute()
        {
            return CanExecute(null);
        }

        private DelegateCommand(Func<Task> executeMethod)
            : this(executeMethod, () => true)
        {
        }

        private DelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Application.Current.FindResource("DelegateCommandDelegatesCannotBeNull").ToString());
        }
    }

}
