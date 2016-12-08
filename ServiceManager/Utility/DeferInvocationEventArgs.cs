using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Contains instructions on deferring or handling a deferred Web Service invocation.
    /// </summary>
    public class DeferInvocationEventArgs : EventArgs
    {
        internal bool ExecuteInBackground { get; private set; }
        internal Action<object> BackgroundExecutionCallback { get; private set; }

        /// <summary>
        /// Gets or sets the exception thrown during the invocation.
        /// </summary>
        /// <value>
        /// The thrown exception.
        /// </value>
        public Exception ThrownException { get; set; }

        /// <summary>
        /// Gets or sets the name of the web service method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; set; }
        /// <summary>
        /// Gets or sets the arguments passed to the method.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public object Arguments { get; set; }

        public void DeferToBackgroundThread(Action<object> onCompleted)
        {
            ExecuteInBackground = true;
            BackgroundExecutionCallback = onCompleted;
        }

        internal DeferInvocationEventArgs(string method, object arg)
        {
            MethodName = method;
            Arguments = arg;
        }
    }
}
