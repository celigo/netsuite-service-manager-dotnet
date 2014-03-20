using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.celigo.net.ServiceManager.Exceptions
{
    /// <summary>
    /// Error thrown when WS operations are locked down due to a previous error.
    /// </summary>
    [Serializable]
    public class WebservicesLockdownException : Exception
    {
        public WebservicesLockdownException() { }
        public WebservicesLockdownException(string message) : base(message) { }
        public WebservicesLockdownException(string message, Exception inner) : base(message, inner) { }
        protected WebservicesLockdownException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
