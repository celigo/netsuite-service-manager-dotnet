using System;

namespace com.celigo.net.ServiceManager.Exceptions
{
	/// <summary>
	/// Thrown when <see cref="NetSuiteServiceManager"/> attempts to login with invalid credentials.
	/// </summary>
	[global::System.Serializable]
	public class InvalidCredentialException : NsException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCredentialException"/> class.
		/// </summary>
		/// <param name="message">The erorr description.</param>
		public InvalidCredentialException(string message) : base(message) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCredentialException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The encapsulated exception.</param>
		public InvalidCredentialException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCredentialException"/> class.
		/// </summary>
		/// <param name="inner">The encapsulated exception.</param>
		public InvalidCredentialException(Exception inner) : base(inner) { }
	}
}
