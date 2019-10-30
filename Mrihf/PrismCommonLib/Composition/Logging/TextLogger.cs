// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using CommonLib.Logs;

namespace PrismCommonLib.Composition.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILog"/> that logs into a <see cref="TextWriter"/>.
    /// </summary>
    public class TextLogger : ILog, IDisposable
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of <see cref="TextLogger"/> that writes to
        /// the console output.
        /// </summary>
        public TextLogger()
            : this(Console.Out)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TextLogger"/>.
        /// </summary>
        /// <param name="writer">The writer to use for writing log entries.</param>
        public TextLogger(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            this.writer = writer;
        }
        /// <summary>
        /// Disposes the associated <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, disposes the associated <see cref="TextWriter"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Log(string message, LogLevel logLevel, Priority priority = Priority.None)
        {
            string messageToLog = String.Format(CultureInfo.InvariantCulture, Application.Current.FindResource("DefaultTextLoggerPattern").ToString(), DateTime.Now,
                                                logLevel.ToString().ToUpper(CultureInfo.InvariantCulture), message, priority.ToString());

            writer.WriteLine(messageToLog);
        }
    }
}