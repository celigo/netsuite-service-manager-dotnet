using System;

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Token used by the Service Pool to maintain state between a search and 
    /// searchMore, searchNext operations.
    /// </summary>
    public class SearchSession : IDisposable
    {
        private IServiceManager _svcMgr;

        internal int PageIndex { get; set; }
        internal string SearchId { get; set; }

        /// <summary>
        /// Gets or sets the Service Manager that initiated the search operation.
        /// </summary>
        /// <value>The Service Manager.</value>
        internal IServiceManager ServiceManager
        {
            get 
            { 
                if (_svcMgr == null)
                    _svcMgr = _svcPoolMgr.GetServiceManager();
                return _svcMgr; 
            }
            set { _svcMgr = value; }
        }

        private NetSuiteServicePoolManager _svcPoolMgr;

        /// <summary>
        /// Gets or sets the Service Pool Manager.
        /// </summary>
        /// <value>The Service Pool Manager.</value>
        internal NetSuiteServicePoolManager ServicePoolManager
        {
            get { return _svcPoolMgr; }
            set { _svcPoolMgr = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSession"/> class.
        /// </summary>
        internal SearchSession()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSession"/> class.
        /// </summary>
        /// <param name="poolManager">The Service Pool Manager.</param>
        internal SearchSession(NetSuiteServicePoolManager poolManager)
        {
            _svcPoolMgr = poolManager;
        }

        /// <summary>
        /// Releases the bound Service Manager instance.
        /// </summary>
        ~SearchSession()
        {
            Dispose();
        }

        #region IDisposable Members

        /// <summary>
        /// Releases the bound Service Manager instance.
        /// </summary>
        public void Dispose()
        {
            if (_svcPoolMgr != null && _svcMgr != null)
            {
                _svcPoolMgr.ReleaseServiceManager(_svcMgr);
                // prevent multiple disposals.
                _svcMgr	 = null;
                _svcPoolMgr = null;
            }
        }

        #endregion
    }
}
