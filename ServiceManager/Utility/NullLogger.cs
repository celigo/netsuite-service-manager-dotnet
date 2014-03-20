using System;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Non-logging logger class serving as a placeholder when logging is unnecessary.
    /// </summary>
    public class NullLogger : ILogger
    {
        #region ILogger Members

        /// <summary>
        /// Gets whether this instance debug messages will be logged.
        /// </summary>
        /// <value></value>
        public bool IsDebugEnabled { get { return false; } }

        /// <summary>
        /// Writes the specified message Info log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message){}

        /// <summary>
        /// Writes the specified message to the Debug log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
        }

        /// <summary>
        /// Writes the specified message to the Debug log..
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Debug(string message, Exception exception)
        {
        }

        /// <summary>
        /// Writes the specified message Warnings log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
        }

        /// <summary>
        /// Writes the specified message Warnings log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warn(string message, Exception exception)
        {
        }

        /// <summary>
        /// Writes the specified message Error log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
        }

        /// <summary>
        /// Writes the specified message Error log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
        }

        #endregion
    }
}
