using System;

namespace com.celigo.net.ServiceManager
{
    /// <summary>Exception thrown by classes within the ServiceManager library.</summary>
    [Serializable]
    public class NsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NsException"/> class.
        /// </summary>
        public NsException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NsException"/> class.
        /// </summary>
        /// <param name="message">The error description.</param>
        public NsException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NsException"/> class.
        /// </summary>
        /// <param name="message">The error description.</param>
        /// <param name="inner">The actual exception wrapped by <see cref="NsException" />.</param>
        public NsException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NsException"/> class.
        /// </summary>
        /// <param name="innerException">The actual exception wrapped by <see cref="NsException" />.</param>
        public NsException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NsException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected NsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
