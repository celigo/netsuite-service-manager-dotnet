using System;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Holds data regarding a CredentialsRequired event.
    /// </summary>
    public class CredentialsRequiredEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the credentials that the Service Manager will use for the WS operation.
        /// </summary>
        /// <value>
        /// The NetSuite credentials.
        /// </value>
        public NetSuiteCredential Credentials { get; set; }
    }
}
