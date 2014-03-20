using System;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Exceptions
{
    /// <summary>
    /// Represents an exception caused by a error reported by NetSuite WS.
    /// </summary>
    [Serializable]
    public class StatusErrorException : Exception
    {
        /// <summary>
        /// Gets the details associated with the error status.
        /// </summary>
        public StatusDetail[] Details 
        { 
            get
            {
                return this.Data["Details"] as StatusDetail[];
            }
            internal set
            {
                this.Data["Details"] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusErrorException"/> class.
        /// </summary>
        public StatusErrorException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public StatusErrorException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public StatusErrorException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusErrorException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        ///   </exception>
        protected StatusErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
