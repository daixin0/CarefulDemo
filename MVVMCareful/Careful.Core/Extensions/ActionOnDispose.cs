using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Careful.Core.Extensions
{
    
    public sealed class ActionOnDispose : IDisposable
    {
        
        public ActionOnDispose(Action unlockAction)
        {
            Contract.Requires(unlockAction != null);

            m_unlockDelegate = unlockAction;
        }

        
        public void Dispose()
        {
            Action action = Interlocked.Exchange(ref m_unlockDelegate, null);
            action();
        }

        private Action m_unlockDelegate;

    }
}
