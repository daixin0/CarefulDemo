using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Core.Mvvm.VMBase
{
    public static class MvvmHelpers
    {
        public static void ViewAndViewModelAction<T>(object view, Action<T> action) where T : class
        {
            T viewAsT = view as T;
            if (viewAsT != null)
                action(viewAsT);
            var element = view as FrameworkElement;
            if (element != null)
            {
                var viewModelAsT = element.DataContext as T;
                if (viewModelAsT != null)
                {
                    action(viewModelAsT);
                }
            }
        }
    }
}
