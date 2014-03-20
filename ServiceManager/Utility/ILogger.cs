using System;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>Defines the interface against which the library components log messages.</summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets whether this instance debug messages will be logged.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Writes the specified message to the Debug log.
        /// </summary>
        /// <param name="message">The message.</param>    
        void Debug(string message);

        /// <summary>
        /// Writes the specified message to the Debug log.
        /// </summary>
        /// <param name="message">The message.</param>    
        /// <param name="exception">The exception.</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Writes the specified message Warnings log.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(string message);
        /// <summary>        
        /// Writes the specified message Warnings log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(string message, Exception exception);

        /// <summary>
        /// Writes the specified message Error log.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);
        /// <summary>
        /// Writes the specified message Error log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Writes the specified message Info log.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);
    }
}
