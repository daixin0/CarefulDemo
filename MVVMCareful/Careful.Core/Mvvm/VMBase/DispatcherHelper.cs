using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Core.Mvvm.VMBase
{
    public static class DispatcherHelper
    {
        public static void InvokeOnUI(Action action) => Application.Current.Dispatcher.Invoke(action);

        public static async Task InvokeOnUIAsync(Action action) => await Task.Run(() => Application.Current.Dispatcher.Invoke(action));

        /// <summary>
        /// 确保在原来的线程上调用回调
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public static void InvokeCallback(this SynchronizationContext context, SendOrPostCallback callback, object state)
        {
            if (context != SynchronizationContext.Current)
            {
                context.Post(callback, state);
            }
            else
            {
                callback(state);
            }
        }
    }
}
