
using System;
namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Represents options for cancelable service invocations.
    /// </summary>
    public class ServiceInvocationEventArgs : System.ComponentModel.CancelEventArgs
    {
        /// <summary>
        /// Gets or sets the name of the method invoked/being invoked.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName { get; private set; }
        /// <summary>
        /// Gets or sets the arguments supplied for the method.
        /// </summary>
        /// <value>The arguments.</value>
        public object Arguments { get; private set; }

        /// <summary>Gets or sets the result of the method invocation if applicable.</summary>
        /// <value>The result.</value>
        /// <remarks>
        /// The value will be <c>null</c> for both <see cref="NetSuiteServiceBase.BeforeServiceInvocation"/> 
        /// and <see cref="NetSuiteServiceBase.ServiceInvocationError"/> events.
        /// </remarks>
        public object Result { get; internal set; }

        /// <summary>
        /// Represents a data slot which can be used by consumers of the Service Manager library to
        /// pass data between <see cref="NetSuiteServiceBase.BeforeServiceInvocation"/>, 
        /// <see cref="NetSuiteServiceBase.AfterServiceInvocation"/> and 
        /// <see cref="NetSuiteServiceBase.ServiceInvocationError"/> events.
        /// </summary>
        /// <value>The user data.</value>
        public object UserData { get; set; }

        /// <summary>
        /// Gets or sets the exception thrown during the invocation.
        /// </summary>
        /// <value>The exception.</value>
        /// <remarks>
        /// Non-<c>null</c> only in <see cref="NetSuiteServiceBase.ServiceInvocationError"/> event and in 
        /// other events when retrying after an exception.
        /// </remarks>
        public Exception Exception { get; internal set; }

        internal int InvokationAttempt { get; set; }

        /// <summary>
        /// Gets or sets whether an immediate retry should be performed.
        /// </summary>
        /// <remarks>This property is ignored if <see cref="System.ComponentModel.CancelEventArgs.Cancel"/> 
        /// is set to <c>true</c>.</remarks>
        public bool ForceRetry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw the original exception or (default) 
        /// wrap the original in a <see cref="NsException"/>.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the original exception should be re-thrown; otherwise, <c>false</c>.
        /// </value>
        public bool ReThrowOriginalException { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInvocationEventArgs"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The args.</param>
        internal ServiceInvocationEventArgs(string method, object args) : base(false)
        {
            this.MethodName  = method;
            this.Arguments   = args;
        }
    }
}
