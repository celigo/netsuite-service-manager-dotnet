using System;

namespace com.celigo.net.ServiceManager.Exceptions
{
    /// <summary>
    /// Thrown when a Service Manager or a Service Pool is incorrectly configured.
    /// </summary>
    [Serializable]
    public class IncorrectConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncorrectConfigurationException"/> class.
        /// </summary>
        public IncorrectConfigurationException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IncorrectConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public IncorrectConfigurationException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IncorrectConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public IncorrectConfigurationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IncorrectConfigurationException"/> class.
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
        protected IncorrectConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
