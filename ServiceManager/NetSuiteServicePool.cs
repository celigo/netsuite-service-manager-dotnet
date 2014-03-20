using System;
using com.celigo.net.ServiceManager.SuiteTalk;
using com.celigo.net.ServiceManager.Utility;

namespace com.celigo.net.ServiceManager
{
    /// <summary>A wrapper of the NetSuiteServiceManager</summary>
    /// <remarks>
    /// <para>
    /// A wrapper of the NetSuiteServiceManager class that provides the following functionality.<br/><br/>  
    /// 1.)  Concurrent Request Processing Across Multiple NetSuite Service Managers.<br/><br/>
    /// 
    /// The NetSuiteServicePool class extends the functionality of the NetSuiteServiceManager class to <br/>
    /// provide the ability to submit requests across multiple service managers.  This class is very <br/>
    /// useful for applications that need more than one NetSuite web services session.  Stateless requests <br/>
    /// can be made directly against the pool.  State-full requests can be made by first allocating an <br/>
    /// available NetSuiteServiceManager instance—don’t forget to release it when done.
    /// </para>
    /// </remarks>
    public class NetSuiteServicePool : NetSuiteServiceBase, IServicePool
    {
        /// <summary>Utility for logging errors and warnings.</summary>
        private readonly ILogger _log;

        private NetSuiteServicePoolManager _svcPoolMgr;

        /// <summary>Gets or sets the Service Pool Manager.</summary>
        /// <value>The Service Pool Manager.</value>
        public NetSuiteServicePoolManager ServicePoolManager
        {
            get { return _svcPoolMgr; }
            set { _svcPoolMgr = value; }
        }

        /// <summary>
        /// Changes the email or password.
        /// </summary>
        /// <param name="currentCredentials">The current credentials.</param>
        /// <param name="cpec">The credentials required for the email change.</param>
        /// <returns></returns>
        public SessionResponse ChangeEmail(NetSuiteCredential currentCredentials, ChangeEmail cpec)
        {
            var svcMgr = _svcPoolMgr.BuildTemporaryServiceManager();
            svcMgr.Credentials = currentCredentials;
            return svcMgr.ChangeEmail(cpec);
        }

        /// <summary>
        /// Changes the email or password.
        /// </summary>
        /// <param name="currentCredentials">The current credentials.</param>
        /// <param name="cpec">The credentials required for the email change.</param>
        /// <returns></returns>
        public SessionResponse ChangePassword(NetSuiteCredential currentCredentials, ChangePassword cpec)
        {
            var svcMgr = _svcPoolMgr.BuildTemporaryServiceManager();
            svcMgr.Credentials = currentCredentials;
            return svcMgr.ChangePassword(cpec);
        }        

        /// <summary>Searches using the specified search options.</summary>
        /// <param name="record">The search record</param>
        /// <returns>Response from the WebService.</returns>
        public override SearchResult Search(SearchRecord record)
        {
            return InvokeService<SearchResult>(record, "search", null);
        }

        /// <summary>
        /// Searches the specified search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="returnSearchColumns"></param>
        /// <returns>The search result.</returns>
        public override SearchResult Search(SearchRecord search, bool returnSearchColumns)
        {
            var searchPrefs = _svcPoolMgr.ServiceConfiguration.SearchPreferences.Duplicate();
            searchPrefs.returnSearchColumns = returnSearchColumns;
            return Search(search, searchPrefs);
        }

        #region : Search Methods :

        /// <summary>
        /// Issues a search session.
        /// </summary>
        /// <returns>A search session.</returns>
        public SearchSession BeginPaginatedSearch()
        {
            try
            {
                return new SearchSession(_svcPoolMgr);
            }
            catch (Exception ex)
            {
                throw new NsException(ex);
            }
        }

        /// <summary>
        /// Searches NetSuite using the specified search parameters.
        /// </summary>
        /// <param name="searchRec">The search parameter.</param>
        /// <param name="prefs">The search preferences.</param>
        /// <returns>Results of the search.</returns>
        public override SearchResult Search(SearchRecord searchRec, SearchPreferences prefs)
        {
            return InvokeService<SearchResult>(searchRec, "search", prefs);
        }

        /// <summary>
        /// Invokes NetSuite's search(..) method.
        /// </summary>
        /// <param name="searchRec">The search options.</param>
        /// <param name="session">The session to which the search and subsequent SearchMore(..),
        /// SearchNext(..) operations will belong.</param>
        /// <returns>Response from the WebService</returns>
        /// <remarks>Use this method only when SearchMore(..) or
        /// SearchNext(..) etc. calls are required.</remarks>
        public SearchResult Search(SearchRecord searchRec, SearchSession session)
        {
            try
            {
                SearchResult result = session.ServiceManager.Search(searchRec);
                session.PageIndex   = result.pageIndex;
                session.SearchId    = result.searchId;
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                throw new NsException(ex);
            }
        }

        /// <summary>Invokes NetSuite's searchMore(..) method.</summary>
        /// <param name="pageIndex">The result page index.</param>
        /// <param name="session">The session in which the original search was performed.</param>
        /// <returns></returns>
        public SearchResult SearchMore(int pageIndex, SearchSession session)
        {
            try
            {
                SearchResult result = session.ServiceManager.SearchMoreWithId(session.SearchId, pageIndex);
                session.SearchId    = result.searchId;
                session.PageIndex   = result.pageIndex;
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                throw new NsException(ex);
            }     
        }

        /// <summary>
        /// Retrieves a page from a Search originally executed using the <paramref name="session"/> token.
        /// </summary>
        /// <param name="pageIndex">Index of the result page to be retrieved.</param>
        /// <param name="prefs">Preference settings to be used for the search.</param>
        /// <param name="session">The search session.</param>
        /// <returns>Results on the specified page.</returns>
        public SearchResult SearchMore(int pageIndex, SearchPreferences prefs, SearchSession session)
        {
            try
            {
                SearchResult result = session.ServiceManager.SearchMoreWithId(session.SearchId, pageIndex, prefs);
                session.SearchId    = result.searchId;
                session.PageIndex   = result.pageIndex;
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                throw new NsException(ex);
            }   
        }

        /// <summary>
        /// Logins in to NetSuite using the Credentials provided.
        /// </summary>
        /// <param name="credential">NetSuite user's Credentials.</param>
        /// <returns>Response from NetSuite.</returns>
        public override IUserSession CreateSession(NetSuiteCredential credential)
        {
            var serviceManager = ServicePoolManager.BuildTemporaryServiceManager();
            return serviceManager.CreateSession(credential);
        }

        /// <summary>
        /// Logout from the specified session.
        /// </summary>
        /// <param name="session">The active session.</param>
        public override void CloseSession(IUserSession session)
        {
            var serviceManager = ServicePoolManager.BuildTemporaryServiceManager();
            serviceManager.CloseSession(session);
        }

        #endregion

        /// <summary>
        /// Invokes the Web Service.
        /// </summary>
        /// <param name="arg">Argument(s) to be passed on to the remote method.</param>
        /// <param name="method">The remote method to be invoked.</param>
        /// <param name="prefs">Search preferences</param>
        /// <returns>The result of the WebService call.</returns>
        /// <remarks>When invoking a remote method that accepts multiple parameters, you should
        /// provide an <see cref="System.Object"/> array as <paramref name="arg"/>.</remarks>
        internal override T InvokeService<T>(object arg, string method, SearchPreferences prefs)
        {
            IServiceManager svcMgr = null;
            ServiceInvocationEventArgs invokerArgs = new ServiceInvocationEventArgs(method, arg);

            try
            {
                OnBeforeServiceInvocation(invokerArgs);
                
                svcMgr = ServicePoolManager.GetServiceManager();
                
                T result;
                if (svcMgr is NetSuiteServiceManager)
                    result = ((NetSuiteServiceManager)svcMgr).InvokeService<T>(arg, method, prefs);
                else
                    result = null;

                ServicePoolManager.ReleaseServiceManager(svcMgr);
                svcMgr = null;

                invokerArgs.Result = result;
                OnAfterServiceInvocation(invokerArgs);
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                invokerArgs.Exception = ex;
                OnServiceInvocationError(invokerArgs);
                throw new NsException(ex);
            }
            finally
            {
                if (svcMgr != null)
                {
                    ServicePoolManager.ReleaseServiceManager(svcMgr);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServicePool"/> class.
        /// </summary>
        public NetSuiteServicePool() : this(new NetSuiteServicePoolManager())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServicePool"/> class.
        /// </summary>
        /// <param name="svcPoolMgr">The manager for this service pool.</param>
        public NetSuiteServicePool(NetSuiteServicePoolManager svcPoolMgr) : this(svcPoolMgr, new NullLogger())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServicePool"/> class.
        /// </summary>
        /// <param name="svcPoolMgr">The manager for this service pool.</param>
        /// <param name="logger">The logger.</param>
        public NetSuiteServicePool(NetSuiteServicePoolManager svcPoolMgr, ILogger logger)
        {
            _log = logger;
            _svcPoolMgr = svcPoolMgr;
        }
    }
}
