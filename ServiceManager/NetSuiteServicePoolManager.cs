using System;
using System.Collections.Generic;
using com.celigo.net.ServiceManager.Exceptions;
using com.celigo.net.ServiceManager.Utility;
#if CLR_2_0
using Celigo.Linq;
#endif

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Maintains a pool of NetSuite Service Managers.
    /// </summary>
    public class NetSuiteServicePoolManager
    {
#if !FIRSTBUILD

        /// <summary>
        /// Gets or sets the configuration under which the operations should be run.
        /// </summary>
        /// <value>The service configuration.</value>
        public NetSuiteServiceConfiguration ServiceConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the credentials available.
        /// </summary>
        /// <value>The credentials available.</value>
        public List<NetSuiteCredential> Credentials { get; set; }

        private BlockingQueue<IServiceManager> _freeServiceManagers;

        private bool _initialized = false;
        private readonly Func<IServiceManager> _factoryMethod;

        private void Initialize()
        {
            if (!_initialized)
                lock (this)
                {
                    if (_initialized)
                        return;

                    CheckConstraints();

                    _freeServiceManagers = new BlockingQueue<IServiceManager>(Credentials.Count);

                    IServiceManager svcMgr;
                    foreach (NetSuiteCredential credential in Credentials)
                    {
                        for (int j = 0; j < credential.NoOfSeats; j++)
                        {
                            svcMgr = GenerateServiceManager(credential);
                            _freeServiceManagers.Enqueue(svcMgr);
                        }
                    }
                    _initialized = true;
                }
        }

        private IServiceManager GenerateServiceManager(NetSuiteCredential credential)
        {
            IServiceManager svcMgr = _factoryMethod.Invoke();
            svcMgr.Credentials = credential;
            svcMgr.Configuration = ServiceConfiguration ?? new NetSuiteServiceConfiguration();
            return svcMgr;
        }

        private void CheckConstraints()
        {
            if (_factoryMethod == null)
                throw new IncorrectConfigurationException("Service Manager factory is null.");
            else if (Credentials == null || Credentials.Count == 0)
                throw new IncorrectConfigurationException("You must specify at least one NetSuite credential before invoking this method.");
        }


        /// <summary>Gets the available service managers.</summary>
        /// <returns>An iterator for the Service Managers that are not being used.</returns>
        /// <remarks>Each ServiceManager returned by this method should be released as usual. Also note that the
        /// method will block until all the ServiceManagers initially constructed are available.</remarks>
        internal IEnumerator<IServiceManager> GetAvailableServiceManagers()
        {
            if (!_initialized)
                Initialize();
            return _freeServiceManagers.GetEnumerator();
        }

        /// <summary>
        /// Builds a Service Manager for one-time use.
        /// </summary>
        /// <returns>A non-initialized <see cref="IServiceManager"/> instance.</returns>
        /// <remarks>
        /// Used for operations that do not require explicit NetSuite credentials. Such as
        /// the <see cref="INetSuiteServiceBase.CreateSession(NetSuiteCredential)"/> method.
        /// By building a temporary IServiceManager instance, we prevent these operations from
        /// unnecessarily tying up available sessions.
        /// </remarks>
        internal IServiceManager BuildTemporaryServiceManager()
        {
            return GenerateServiceManager(null);
        }

        /// <summary>
        /// Gets a un-utilized Service Manager.
        /// </summary>
        /// <returns>A free Service Manager instance.</returns>
        public IServiceManager GetServiceManager()
        {
            if (!_initialized)
                Initialize(); // Minimize the bottleneck at Initialize() method.
            return _freeServiceManagers.Dequeue();
        }

        /// <summary>
        /// Releases the Service Manager making it available for subsequent 
        /// <see cref="GetServiceManager()"/> calls.
        /// </summary>
        /// <param name="svcMgr">The Service Manager to be released.</param>
        public void ReleaseServiceManager(IServiceManager svcMgr)
        {
            if (!_initialized) Initialize();
            _freeServiceManagers.Enqueue(svcMgr);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServicePoolManager"/> class.
        /// </summary>
        public NetSuiteServicePoolManager() : this( () => new NetSuiteServiceManager() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServicePoolManager"/> class.
        /// </summary>
        /// <param name="factoryMethod">The factory method to be invoked when generating ServiceManagers for the current pool.</param>
        public NetSuiteServicePoolManager(Func<IServiceManager> factoryMethod)
        {
            ServiceConfiguration  = new NetSuiteServiceConfiguration();
            Credentials = new List<NetSuiteCredential>(1);

            _factoryMethod = factoryMethod;
        }
#endif
    }
}
