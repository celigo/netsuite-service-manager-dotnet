using System;

namespace com.celigo.net.ServiceManager.Exceptions
{
    /// <summary>
    /// Represents the execption thrown when <see cref="NetSuiteServiceManager"/> has retried the the configured
    /// number of times.
    /// </summary>
	[global::System.Serializable]
	public class RetryCountExhaustedException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryCountExhaustedException"/> class.
        /// </summary>
		public RetryCountExhaustedException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryCountExhaustedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
		public RetryCountExhaustedException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryCountExhaustedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
		public RetryCountExhaustedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryCountExhaustedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
		protected RetryCountExhaustedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
