#if !FIRSTBUILD

using System;
using com.celigo.net.ServiceManager.SuiteTalk;
using com.celigo.net.ServiceManager.Utility;

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Base of all ServiceManager library classes that invokes NetSuite web service methods directly
    /// or indirectly.
    /// </summary>
    public abstract partial class NetSuiteServiceBase : INetSuiteServiceBase
    {
        /// <summary>
        /// Occurs before invoking a NS WS API method.
        /// </summary>
        public event EventHandler<ServiceInvocationEventArgs> BeforeServiceInvocation;

        /// <summary>
        /// Occurs when after a WS API method is invoked.
        /// </summary>
        public event EventHandler<ServiceInvocationEventArgs> AfterServiceInvocation;

        /// <summary>
        /// Occurs when a WS API method throws an error.
        /// </summary>
        public event EventHandler<ServiceInvocationEventArgs> ServiceInvocationError;

        #region : Service Operations :

        /// <summary>
        /// Logout from the specified session.
        /// </summary>
        /// <param name="session">The active session.</param>
        public abstract void CloseSession(IUserSession session);

        /// <summary>
        /// Logins in to NetSuite using the Credentials provided.
        /// </summary>
        /// <param name="credential">NetSuite user's Credentials.</param>
        /// <returns>Response from NetSuite.</returns>
        public abstract IUserSession CreateSession(NetSuiteCredential credential);

        /// <summary>
        /// Searches using the specified search options.
        /// </summary>
        /// <param name="record">The search record.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual SearchResult Search(SearchRecord record)
        {
            return Search(record, null);
        }

        /// <summary>
        /// Searches the specified search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="returnSearchColumns">if set to <c>true</c> returns search columns instead of entities.</param>
        /// <returns>The search result.</returns>
        public abstract SearchResult Search(SearchRecord search, bool returnSearchColumns);

        /// <summary>
        /// Searches using the specified search options.
        /// </summary>
        /// <param name="record">The search record.</param>
        /// <param name="prefs">Search Preferences</param>
        /// <returns>Response from the WebService.</returns>
        public abstract SearchResult Search(SearchRecord record, SearchPreferences prefs);

        #endregion

        /// <summary>
        /// Strongly typed wrapper to the <see cref="InvokeService(object, string)"/> call.
        /// </summary>
        /// <typeparam name="T">Return type of the WebService call.</typeparam>
        /// <param name="arg">Argument(s) to be passed on to the remote method.</param>
        /// <param name="method">The remote method to be invoked.</param>
        /// <param name="prefs">Search Preferences to be used for this invocation.</param>
        /// <returns>The result of the WebService call.</returns>
        /// <remarks>IMPORTANT: This wrapper should not be used with remote methods that does
        /// not accept any parameters.</remarks>
        internal abstract T InvokeService<T>(object arg, string method, SearchPreferences prefs) where T : class;

        internal virtual T InvokeService<T>(object arg, string method) where T: class
        {
            return (T)InvokeService<T>(arg, method, null);
        }

        /// <summary>
        /// Occurs before a remote NetSuite Method is invoked. Deriving classes may override
        /// this method to perform additional processing before each Web Service invocation.
        /// </summary>
        /// <param name="e">The <see cref="com.celigo.net.ServiceManager.Utility.ServiceInvocationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnBeforeServiceInvocation(ServiceInvocationEventArgs e)
        {
            if (BeforeServiceInvocation != null)
                BeforeServiceInvocation(this, e);
        }

        /// <summary>
        /// Occurs after a remote NetSuite Method is invoked. Deriving classes may override
        /// this method to perform additional processing after each Web Service invocation.
        /// </summary>
        /// <param name="e">The <see cref="com.celigo.net.ServiceManager.Utility.ServiceInvocationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAfterServiceInvocation(ServiceInvocationEventArgs e)
        {
            if (AfterServiceInvocation != null)
                AfterServiceInvocation(this, e);
        }

        /// <summary>
        /// Occurs when an exception occurs during a NetSuite operation.
        /// </summary>
        /// <param name="e">The <see cref="com.celigo.net.ServiceManager.Utility.ServiceInvocationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnServiceInvocationError(ServiceInvocationEventArgs e)
        {
            if (ServiceInvocationError !=null)
                ServiceInvocationError(this, e);
        }
    }
}

#endif
