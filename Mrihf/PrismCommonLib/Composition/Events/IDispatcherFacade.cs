// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;

namespace PrismCommonLib.Composition.Events
{
    /// <summary>
    /// Defines the interface for invoking methods through a Dispatcher Facade
    /// </summary>
    [Obsolete]
    public interface IDispatcherFacade
    {
        /// <summary>
        /// Dispatches an invocation to the method received as parameter.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        void BeginInvoke(Delegate method, object arg);
    }
}