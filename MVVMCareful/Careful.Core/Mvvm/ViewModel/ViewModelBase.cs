using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Careful.Core.Mvvm.PropertyChanged;

namespace Careful.Core.Mvvm.ViewModel
{
    public abstract class ViewModelBase : NotifyPropertyChanged, ICleanup
    {
        public virtual void Cleanup()
        {
            //MessengerInstance.Unregister(this);
        }

    }
}
