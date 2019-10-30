// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using CommonLib.Logs;
using System.Diagnostics;

namespace PrismCommonLib.Composition.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILog"/> that logs to .NET <see cref="Trace"/> class.
    /// </summary>
    public class TraceLogger : ILog
    {
        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="message">Message body to log.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="priority">The priority of the entry.</param>
        public void Log(string message, LogLevel category, Priority priority)
        {
            if (category == LogLevel.Exception)
            {
                Trace.TraceError(message);
            }
            else
            {
                Trace.TraceInformation(message);
            }
        }
    }
}