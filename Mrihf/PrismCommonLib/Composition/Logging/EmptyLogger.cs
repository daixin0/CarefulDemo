// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.


using CommonLib.Logs;

namespace PrismCommonLib.Composition.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILog"/> that does nothing. This
    /// implementation is useful when the application does not need logging
    /// but there are infrastructure pieces that assume there is a logger.
    /// </summary>
    public class EmptyLogger : ILog
    {
        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="message">Message body to log.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="priority">The priority of the entry.</param>
        public void Log(string message, LogLevel category, Priority priority)
        {

        }
        
    }
}