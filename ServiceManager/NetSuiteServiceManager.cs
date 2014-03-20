using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Xml;
using com.celigo.net.ServiceManager.Exceptions;
using com.celigo.net.ServiceManager.SuiteTalk;
using com.celigo.net.ServiceManager.Utility;
#if CLR_2_0
using Celigo.Linq;
#endif

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// A wrapper of the generated SuiteTalk proxy class
    /// </summary>
    /// <remarks>
    /// A wrapper of the generated SuiteTalk proxy class that provides the following functionality.<br/><br/>
    /// 1.)  Batch Processing.<br/>
    /// 2.)  Robust Request Processing.<br/>
    /// 3.)  Concurrent Request Processing.<br/><br/>
    /// The NetSuiteServiceManager class provides a very useful interface to the operations supported by <br/>
    /// NetSuite's web services.  All session management is seamlessly handled under the covers including <br/>
    /// logging in, session validation, fail and retry attempts, and more.  The service manager class is 100% <br/>
    /// thread safe instantiate one object and share it with as many threads as needed.  Built in support <br/>
    /// for batch processing is also included at instantiation, simply specify the desired batch sizes for <br/>
    /// adds, updates, and deletes.  This class is a great starting point for any application that <br/>
    /// needs access to NetSuite's web services.
    /// </remarks>
    public partial class NetSuiteServiceManager : NetSuiteServiceBase, IServiceManager
    {
        /// <summary>Utility for logging errors and warnings.</summary>
        private readonly ILogger _log;
        private string _virtualSessionId;

        /// <summary>
        /// Gets the service proxy.
        /// </summary>
        /// <value>The service proxy.</value>
        protected INetSuiteService ServiceProxy { get; private set; }

        /// <summary>Gets or sets the SuiteTalk webservice URL.</summary>
        [Obsolete("Use NetSuiteServiceManager.ServiceConfiguration.EndPointUrl instead.")]
        public string RemoteUrl
        {
            get { return ServiceProxy.Url; }
            set { ServiceProxy.Url = value; }
        }

        /// <summary>
        /// Gets the service configuration under which the Service Manager
        /// would operate.
        /// </summary>
        /// <value>The service configuration.</value>
        public NetSuiteServiceConfiguration Configuration { get; set; }

        /// <summary>
        /// Occurs before a batch of records is uploaded to NetSuite for AddList, UpdateList or DeleteList
        /// operations.
        /// </summary>
        public event EventHandler<BeforeBatchUploadEventArgs> BeforeBatchUpload;

        /// <summary>
        /// Occurs before a batch of records has been uploaded to NetSuite for AddList, UpdateList or DeleteList
        /// operations.
        /// </summary>
        public event EventHandler<AfterBatchUploadEventArgs> AfterBatchUpload;

        /// <summary>
        /// Occurs before a batch of records are deleted from NetSuite.
        /// </summary>
        public event EventHandler<BeforeBatchDeleteEventArgs> BeforeBatchDelete;

        /// <summary>
        /// Occurs after a batch of records has been deleted from NetSuite.
        /// </summary>
        public event EventHandler<AfterBatchUploadEventArgs> AfterBatchDelete;

        /// <summary>
        /// Occurs before Service Manager encounters empty NS Credentials before executing a WS operation.
        /// </summary>
        public event EventHandler<CredentialsRequiredEventArgs> CredentialsRequired;

        #region : Authentication & Authorization :

        private NetSuiteCredential _credentials;

        /// <summary>
        /// Gets or sets the credentials under which Service Manager would operate.
        /// </summary>
        /// <value>The NetSuite user credentials.</value>
        public virtual NetSuiteCredential Credentials
        {
            get 
            {
                if (_credentials == null && CredentialsRequired != null)
                {
                    var args = new CredentialsRequiredEventArgs();
                    CredentialsRequired(this, args);
                    _credentials = args.Credentials;
                }
                return _credentials; 
            }
            set { _credentials = value; }
        }
        
        /// <summary>
        /// Invokes NetSuite's changeEmail(..) method.
        /// </summary>
        /// <param name="cpec">The credentials required for the password/email change.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual SessionResponse ChangeEmail(ChangeEmail cpec)
        {
            return InvokeService<SessionResponse>(cpec, "changeEmail");
        }

        /// <summary>
        /// Invokes NetSuite's changeEmail(..) method.
        /// </summary>
        /// <param name="cpec">The credentials required for the password/email change.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual SessionResponse ChangePassword(ChangePassword cpec)
        {
            return InvokeService<SessionResponse>(cpec, "changeEmail");
        }

        /// <summary>Performs the actual login</summary>
        /// <returns></returns>
        private SessionResponse ExecuteLogin(INetSuiteService serviceProxy, 
                                                NetSuiteCredential credential,
                                                Action<ServiceInvocationEventArgs> onErrorCallback)
        {
            bool loggedIn = false;

            _log.Debug("Initializing NetSuite Service Proxy", null);
            serviceProxy.CookieContainer  = new CookieContainer();
            Configuration.Configure(serviceProxy);

            SessionResponse ssnResponse = null;
            
            ServiceInvocationEventArgs invokerEventArgs = new ServiceInvocationEventArgs("login", credential);
            for (; invokerEventArgs.InvokationAttempt < Configuration.RetryCount; invokerEventArgs.InvokationAttempt++)
            {
                ssnResponse = TryLogin(serviceProxy, 
                                        credential.GetPassport(), 
                                        _log, 
                                        invokerEventArgs,
                                        onErrorCallback);
                loggedIn = (ssnResponse != null && ssnResponse.status.isSuccessSpecified && ssnResponse.status.isSuccess);
                if (this.IsSuspended)
                    this.IsSuspended = !loggedIn;

                if (loggedIn || invokerEventArgs.Cancel)
                    break;
                else if (!invokerEventArgs.ForceRetry)
                    throw invokerEventArgs.Exception;
                else if (invokerEventArgs.InvokationAttempt != Configuration.RetryCount - 1)
                    WaitForRetryInterval();
            }

            if (!loggedIn)
            {
                ProcessLoginError(ssnResponse, invokerEventArgs);
            }

            return ssnResponse;
        }

        private void ProcessLoginError(SessionResponse ssnResponse, ServiceInvocationEventArgs invokerEventArgs)
        {
            Exception thrownException = invokerEventArgs.Exception;
            StatusDetail errorStatus  = ssnResponse != null 
                                        ? StatusResponse.GetStatusError(ssnResponse.status)
                                        : null;
            if (errorStatus != null)
                thrownException = new NsException(errorStatus.message, thrownException);
            else if (invokerEventArgs.InvokationAttempt == Configuration.RetryCount)
                thrownException =  new RetryCountExhaustedException("Login Failed.", thrownException);
            else // If the _nsSvc.login(..) call repeatedly threw exceptions...
                thrownException = new NsException("Login Failed.\nExceptions have been logged.", thrownException);

            invokerEventArgs.Exception = thrownException;
            if (thrownException != null)
            {
                OnServiceInvocationError(invokerEventArgs);
                throw thrownException;
            }
        }

        private static SessionResponse TryLogin(INetSuiteService serviceProxy,
                                            Passport passport, 
                                            ILogger log,
                                            ServiceInvocationEventArgs invokerEventArgs,
                                            Action<ServiceInvocationEventArgs> onErrorCallback)
        {
            SessionResponse response = null;
            try
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug(string.Format("Logging into NetSuite [Username={0}, Account={1}, RoleId={2}]", 
                                              passport.email, 
                                              passport.account, 
                                              passport.role == null ? null : passport.role.internalId
                                            ));
                }

                invokerEventArgs.Exception = null;
                response = serviceProxy.login(passport);
            }
            catch (SoapException soapEx)
            {
                invokerEventArgs.Exception = soapEx;
                log.Debug("Login Failed", soapEx);

                // if this is a InvalidCredentialFault then there's no point in continuing
                bool isInvalidCredentialError = soapEx.Detail.FirstChild.Name.EndsWith("invalidCredentialsFault")
                                                || soapEx.Detail.InnerText.StartsWith("WS_FEATURE_REQD");
                if (isInvalidCredentialError && null == onErrorCallback)
                {
                    invokerEventArgs.Exception = new InvalidCredentialException(invokerEventArgs.Exception);
                }
                else if (isInvalidCredentialError)
                {
                    invokerEventArgs.Exception = new InvalidCredentialException(invokerEventArgs.Exception);
                    onErrorCallback(invokerEventArgs);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Login Failed", ex);
                invokerEventArgs.Exception = ex;
            }

            invokerEventArgs.Result = response;
            return response;
        }

        private static void ExecuteLogout(INetSuiteService serviceProxy, ILogger log)
        {
            try
            {
                serviceProxy.logout();
                serviceProxy.CookieContainer = null; // Will be set on next method call..
            }
            catch (Exception ex)
            {
                log.Warn("Non critical error while trying to log off", ex);
            }
        }

        #endregion

        #region : Session Management :

        /// <summary>
        /// Logins in to NetSuite using the Credentials provided.
        /// </summary>
        /// <param name="credential">NetSuite user's Credentials.</param>
        /// <returns>Response from NetSuite.</returns>
        public override IUserSession CreateSession(NetSuiteCredential credential)
        {
            var serviceProxy = this.ServiceProxy.Clone();
            var response = ExecuteLogin(serviceProxy, credential, null);
            return new UserSession(serviceProxy, credential, response);
        }

        /// <summary>Logout from the specified session.</summary>
        /// <param name="session">The active session.</param>
        public override void CloseSession(IUserSession session)
        {
            var userSession = session as UserSession;
            if (null == userSession)
                return;

            var serviceProxy = userSession.ServiceProxy;

            if (serviceProxy == null && userSession.SessionId == null)
                throw new NsException("You have already logged out of this session");
            else if (serviceProxy == null)
                serviceProxy = CreateProxyWithSessionId(userSession.SessionId);

            ExecuteLogout(serviceProxy, _log);

            if (userSession.SessionId == _virtualSessionId)
                this.ServiceProxy = this.ServiceProxy.Clone(); // start using a fresh proxy.

            userSession.ServiceProxy = null;
            if (userSession.UserId != null)
                _log.Info("User " + userSession.UserId.internalId + "logged out");
        }

        private INetSuiteService CreateProxyWithSessionId(string sessionId)
        {
            var proxy = this.ServiceProxy.Clone();
            proxy.CookieContainer = new CookieContainer();
            proxy.CookieContainer
                 .Add(new Uri(this.ServiceProxy.Url), new Cookie(UserSession.JSESSIONID, sessionId));
            return proxy;
        }

        /// <summary>
        /// Log out from the current session.
        /// </summary>
        public void CloseSession()
        {
            ExecuteLogout(this.ServiceProxy, _log);
            this.ServiceProxy = this.ServiceProxy.Clone();
        }

        /// <summary>
        /// Adopts a session created using the <see cref="INetSuiteServiceBase.CreateSession(NetSuiteCredential)"/>
        /// method.
        /// </summary>
        /// <param name="session">An active session.</param>
        public void AdoptSession(IUserSession session)
        {
            var userSession = session as UserSession;
            if (null == userSession)
            {
                throw new NsException("This session was not generated by a ServiceManager and therefore, cannot be adopted.");
            }
            //else if (userSession.ServiceProxy == null)
            //{
            //    throw new NsException("Specified session is inactive.");
            //}
            else
            {
                this.ServiceProxy = userSession.ServiceProxy ?? this.ServiceProxy.Clone();
                this.Credentials = userSession.Credentials;
                this._virtualSessionId = userSession.SessionId;
                this.IsSuspended = false;
            }
        }

        #endregion

        #region : Service Base Overrides :

        /// <summary>
        /// Invokes NetSuite's addList(..) method.
        /// </summary>
        /// <param name="records">The records to be added.</param>
        /// <returns>Response from the WebService.</returns>
        public override WriteResponse[] AddList(Record[] records)
        {
            if (records == null || records.Length == 0)
                return new WriteResponse[0];
            return ProcessRecordAddsInBatchMode(records, "addList");
        }

        /// <summary>
        /// Invokes NetSuite's deleteList(..) method.
        /// </summary>
        /// <param name="baseRefs">The items to be deleted.</param>
        /// <returns>Invokes NetSuite's add(..) method.</returns>
        public override WriteResponse[] DeleteList(BaseRef[] baseRefs)
        {
            if (baseRefs == null || baseRefs.Length == 0)
                return new WriteResponse[0];
            return ProcessRecordDeletesInBatchMode(baseRefs, "deleteList");
        }

        /// <summary>
        /// Invokes NetSuite's updateList(..) method.
        /// </summary>
        /// <param name="records">The records to be updated.</param>
        /// <returns>Response from the WebService.</returns>
        public override WriteResponse[] UpdateList(Record[] records)
        {
            if (records == null || records.Length == 0)
                return new WriteResponse[0];
            return ProcessRecordUpdatesInBatchMode(records, "updateList");
        }

        #region :  InvokeService Method :
        internal override T InvokeService<T>(object arg, string method)
        {
            return InvokeService<T>(arg, method, null);
        }

        /// <summary>Invokes the remote method using the specified parameters and settings.</summary>
        /// <param name="arg">Method arguments</param>
        /// <param name="method">Name of the remote method to execute.</param>
        /// <param name="searchPrefs">Search Preference settings.</param>
        /// <returns>Result of the web service.</returns>
        internal override T InvokeService<T>(object arg, string method, SearchPreferences searchPrefs)
        {
            lock (ServiceProxy)
            {
                Configuration.Configure(ServiceProxy, searchPrefs);

                T result = null;
                MethodInfo mi = ServiceProxy.GetType().GetMethod(method);
                ParameterInfo[] parameters = mi.GetParameters();
                Func<object, T> invokerFunc = GetInvokerFunction<T>(mi, parameters);

                ServiceInvocationEventArgs invokerEventArgs = new ServiceInvocationEventArgs(method, arg);

                for (; invokerEventArgs.InvokationAttempt < Configuration.RetryCount; invokerEventArgs.InvokationAttempt++)
                {
                    ServiceProxy.passport = Credentials.GetPassport();
                    OnBeforeServiceInvocation(invokerEventArgs);

                    if (!invokerEventArgs.Cancel && this.IsSuspended)
                        throw new WebservicesLockdownException();
                    else if (invokerEventArgs.Cancel)
                        break;
                    else if (_log.IsDebugEnabled && parameters.Length == 0)
                        LogInvocationMeaningfully(invokerEventArgs.MethodName);
                    else if (_log.IsDebugEnabled)
                        LogInvocationMeaningfully(invokerEventArgs.MethodName, invokerEventArgs.Arguments);
                    
                    bool retry;
                    result = TryInvokeService<T>(invokerFunc, invokerEventArgs, out retry);
                    if (!retry)
                        break;
                }

                if (invokerEventArgs.InvokationAttempt == Configuration.RetryCount) // Retry count was exhausted..
                {
                    invokerEventArgs.Exception = new RetryCountExhaustedException("Operation Failed", invokerEventArgs.Exception);
                    OnServiceInvocationError(invokerEventArgs);
                    throw invokerEventArgs.Exception;
                }

                return result;
            }
        }

        private T TryInvokeService<T>(Func<object, T> invokerFunc, ServiceInvocationEventArgs invokerEventArgs, out bool retry) where T: class
        {
            T result;
            try
            {
                invokerEventArgs.Result = result = invokerFunc(invokerEventArgs.Arguments);
                OnAfterServiceInvocation(invokerEventArgs);

                retry = false;
                return result;
            }
            catch (Exception ex)
            {
                Exception thrownException = invokerEventArgs.Exception = GetMostRelevantException(ex);

                OnServiceInvocationError(invokerEventArgs);
                if (invokerEventArgs.Cancel)
                {
                    _log.Debug("Retry canceled by user code");
                    retry = false;
                }
                else if (invokerEventArgs.ForceRetry)
                {
                    _log.Debug("Retry forced by user code");
                    retry = true;
                }
                else if (ErrorCanBeWorkedArround(thrownException))
                {
                    _log.Debug(string.Format("Operation Failed with Exception: {0}", thrownException.GetType().Name));
                    _log.Debug(thrownException.ToString());
                    _log.Debug("Attempting Workaround");
                    WaitForRetryInterval();

                    // Since we use request level authentication, this is no longer a solution.
                    if (ErrorRequiresNewLogin(thrownException) || invokerEventArgs.InvokationAttempt >= 4)
                        ExecuteLogin(ServiceProxy, Credentials, OnServiceInvocationError);
                    retry = true;
                }
                else if (ErrorRequiresLockdown(thrownException))
                {
                    _log.Error("WebServices operations will be locked down until an explicit login call is made due to the following error.", ex);
                    throw thrownException;
                }
                else
                {
                    _log.Error(string.Format("Operation Failed with Exception: {0}", thrownException.GetType().Name), thrownException);
                    throw new NsException(thrownException);
                }
                return null;
            }
        }

        private bool ErrorRequiresLockdown(Exception thrownException)
        {
            return thrownException is InvalidCredentialException;
        }

        private Func<object, T> GetInvokerFunction<T>(MethodInfo mi, ParameterInfo[] parameters)
        {
            Func<object, T> invokerFunc;

            if (parameters.Length == 0)
            {
                invokerFunc = new Func<object,T>(arg => (T)mi.Invoke(ServiceProxy, null));
            }
            else if (parameters.Length == 1)
            {
                invokerFunc = new Func<object, T>(arg => (T)mi.Invoke(ServiceProxy, new object[] { arg }));
            }
            else
            {
                invokerFunc = new Func<object,T>(arg => 
                                                    {
                                                        object[] argArray = (object[])arg;
                                                        return (T)mi.Invoke(ServiceProxy, argArray);
                                                    });
            }

            return invokerFunc;
        }

        //private void PrepareServiceProxy(INetSuiteService svc, SearchPreferences searchPrefs)
        //{
        //    if (svc.CookieContainer == null)
        //        svc.CookieContainer  = new CookieContainer();

        //    svc.preferences = Configuration.Preferences;
        //    svc.Timeout  = Timeout.Infinite;

        //    SetSearchPreferences(svc, searchPrefs);
        //}

        //private void SetSearchPreferences(INetSuiteService svc, SearchPreferences searchPrefs)
        //{
        //    if (searchPrefs == null)
        //        svc.searchPreferences = Configuration.SearchPreferences;
        //    else
        //        svc.searchPreferences = searchPrefs;
        //}

        private void LogInvocationMeaningfully(string method)
        {
            if (_log.IsDebugEnabled)
                _log.Debug(string.Format("Invoking '{0}' method", method));
        }

        private void LogInvocationMeaningfully(string method, object arg)
        {
            try
            {
                if (arg is CustomizationType)
                {
                    _log.Debug(string.Format("Invoking method '{0}' with {1}", 
                                            method, 
                                            ((CustomizationType)arg).getCustomizationType.ToString()));
                }
                else if (arg != null && arg is Array && IsGetSelectValueInvocation(arg))
                {
                    Array argArray = (Array)arg;
                    GetSelectValueFieldDescription fieldDesc = (GetSelectValueFieldDescription)argArray.GetValue(0);
                    _log.Debug(string.Format(
                                    "Invoking method '{0}' with '{1}.{2}'",
                                    method,
                                    fieldDesc.customRecordType != null 
                                    ? fieldDesc.customRecordType.internalId
                                    : fieldDesc.recordType.ToString(),
                                    fieldDesc.field));
                }
                else if (arg is CustomRecordSearchBasic)
                {
                    _log.Debug(string.Format(
                                    "Invoking Custom Record Search for RecType={1}",
                                    method,
                                    ((CustomRecordSearchBasic)arg).recType.internalId));
                }
                else if (arg is GetAllRecordType)
                {
                    _log.Debug("Retrieving All Records of Type " + ((GetAllRecordType)arg).ToString());
                }
                else
                    _log.Debug(string.Format("Invoking method '{0}' with {1}", method, arg));
            }
            catch
            {
            }
        }

        private bool IsGetSelectValueInvocation(object arg)
        {
            Debug.Assert(arg is Array);
            Array argArray = (Array)arg;
            return argArray.Length > 0 && argArray.GetValue(0) is GetSelectValueFieldDescription;
        }

        private bool ErrorRequiresNewLogin(Exception ex)
        {
            bool retVal	 = (ex is SoapException) && ((SoapException)ex).Detail.FirstChild.Name.EndsWith("invalidSessionFault");
            // Check whether this is an invalid session exception.
            retVal |= (ex.InnerException is SoapException) && ((SoapException)ex.InnerException).Detail.FirstChild.Name.EndsWith("invalidSessionFault");

            if (retVal) 
                _log.Debug("Error requires a re-login");
            return retVal;
        }

        private bool ErrorCanBeWorkedArround(Exception ex)
        {
            _log.Debug(string.Concat("Checking whether exception", ex.Message, "can be worked around"));

            SoapException soapEx;
            if (IsSoapException(ex, out soapEx))
            {
                var detail = new StringBuilder();
                if (soapEx.Detail != null)
                    using (var writer = XmlTextWriter.Create(detail))
                    {
                        soapEx.Detail.WriteTo(writer);
                    }
                _log.Info("Encountered SoapException: " + detail.ToString());

                return soapEx.Message == "Server Error"
                        || soapEx.Detail.FirstChild.Name.EndsWith("invalidSessionFault") 
                        || soapEx.Detail.FirstChild.Name.EndsWith("exceededRequestLimitFault")
                        || soapEx.Message.Contains("java.lang.NullPointerException");
            }
            else
            {
                return ex is WebException 
                        || ex.InnerException is WebException
                        || ex.Message.Contains("Client found response content type of 'text/html");
            }
        }

        private bool IsSoapException(Exception exception, out SoapException soapException)
        {
            soapException = null;

            if (exception is SoapException)
                soapException = (SoapException)exception;
            else if (exception.InnerException is SoapException)
                soapException = (SoapException)exception.InnerException;

            return null != soapException;
        }

        private Exception GetMostRelevantException(Exception ex)
        {
            Exception parentException = ex;
            while (parentException.InnerException != null && parentException is TargetInvocationException)
            {
                parentException = parentException.InnerException;
            }

            bool isSoapException = parentException is SoapException;
            SoapException soapEx = null;
            // Should this exception be wrapped?
            if (isSoapException && (soapEx = (SoapException)parentException).Detail.FirstChild.Name.EndsWith("invalidCredentialsFault"))
            {
                parentException = new InvalidCredentialException(parentException);
            }
            else if (isSoapException && soapEx.Detail.FirstChild.Name.EndsWith("exceededRequestLimitFault"))
            {
                parentException = new ConcurrentSessionException(parentException);
            }
            return parentException;
        }

        private void WaitForRetryInterval()
        {
            try
            {
                Thread.Sleep(Configuration.RetryInterval * 1000);
            }
            catch (ThreadInterruptedException ex)
            {
                _log.Error(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region : Batch Processing :

        private WriteResponse[] ProcessRecordDeletesInBatchMode(BaseRef[] records, string methodName)
        {
            int batchSize = Configuration.DeleteRequestSize;
            var beforeUploadArgs = new BeforeBatchDeleteEventArgs(methodName, records.Length, batchSize);

            var beforeUploadHandler = GetBeforeDeleteHandler();
            var afterUploadHandler = GetAfterDeleteHandler();

            if (records.Length <= batchSize)
            {
                beforeUploadHandler(beforeUploadArgs.UpdateData(records, 1));
                var result = InvokeService<WriteResponse[]>(records, methodName);
                afterUploadHandler(new AfterBatchUploadEventArgs(beforeUploadArgs, result).UpdateData(0));
                return result;
            }

            List<WriteResponse> responses = new List<WriteResponse>(records.Length);
            var afterUploadArgs = new AfterBatchUploadEventArgs(null, responses);

            BaseRef[] batch = new BaseRef[batchSize];
            int leftOverCount = 0;
            int batchNumber = 1;
            int responseStartIndex;

            for (int i = 0; i < records.Length; i++)
            {
                if (i != 0 && (i%Configuration.DeleteRequestSize == 0))
                {
                    beforeUploadHandler(beforeUploadArgs.UpdateData(batch, batchNumber));
                    responseStartIndex = responses.Count;
                    ProcessDeleteBatch(batch, responses, methodName);
                    afterUploadHandler(afterUploadArgs.UpdateData(responseStartIndex));
                    ++batchNumber;

                    leftOverCount = 0;
                    batch = new BaseRef[Configuration.DeleteRequestSize];
                }
                batch[i%Configuration.DeleteRequestSize] = records[i];
                ++leftOverCount;
            }

            BaseRef[] leftOvers  = new BaseRef[leftOverCount];
            Array.Copy(batch, leftOvers, leftOverCount);

            beforeUploadHandler(beforeUploadArgs.UpdateData(leftOvers, batchNumber));
            responseStartIndex = responses.Count;
            ProcessDeleteBatch(leftOvers, responses, methodName);
            afterUploadHandler(afterUploadArgs.UpdateData(responseStartIndex));
            return responses.ToArray();
        }

        private void ProcessDeleteBatch(BaseRef[] batch, List<WriteResponse> responses, string methodName)
        {
            _log.Debug(string.Format("Processing Batch -- Size: {0} Ref(s), Operation: {1}", batch.Length, methodName));
            try
            {
                responses.AddRange(InvokeService<WriteResponse[]>(batch, methodName));
            }
            catch (Exception e)
            {
                _log.Debug("Processing Batch Error: {0}", e);

                StatusDetail statDetail = new StatusDetail();
                statDetail.message      = e.GetType().Name + " while processing batch: "  + e.Message;
                statDetail.type         = StatusDetailType.ERROR;
                statDetail.code         = StatusDetailCodeType.UNEXPECTED_ERROR;

                Status stat             = new Status();
                stat.isSuccess          = false;
                stat.isSuccessSpecified = true;
                stat.statusDetail       = new StatusDetail[] { statDetail };

                WriteResponse response  = new WriteResponse();
                response.status		 = stat;

                for (int i = 0; i < batch.Length; i++)
                    responses.Add(response);
            }
        }

        private WriteResponse[] ProcessRecordAddsInBatchMode(Record[] records, string methodName)
        {
            if (records.Length <= Configuration.AddRequestSize)
                return DirectInvoke(records, methodName, Configuration.AddRequestSize);
            else
                return BatchInvoke(records, methodName, Configuration.AddRequestSize);
        }

        private WriteResponse[] ProcessRecordUpdatesInBatchMode(Record[] records, string methodName)
        {
            if (records.Length <= Configuration.UpdateRequestSize)
                return DirectInvoke(records, methodName, Configuration.UpdateRequestSize);
            else
                return BatchInvoke(records, methodName, Configuration.UpdateRequestSize);
        }

        private WriteResponse[] BatchInvoke(Record[] records, string methodName, int batchSize)
        {
            if (_log.IsDebugEnabled)
                _log.Debug(string.Format("Executing {0} in batch mode: {1} records, {2} batch size", methodName, records.Length, batchSize));

            var beforeUploadArgs = new BeforeBatchUploadEventArgs(methodName, records.Length, batchSize);

            var beforeUploadHandler = GetBeforeUploadHandler();
            var afterUploadHandler = GetAfterUploadHandler();

            var responses = new List<WriteResponse>(records.Length);
            var afterUploadArgs = new AfterBatchUploadEventArgs(beforeUploadArgs, responses);

            Record[] batch	    = new Record[batchSize];
            int leftOverCount   = 0;
            int batchNumber     = 1;
            int responseStartIndex;

            for (int i = 0; i < records.Length; i++)
            {
                if (i != 0 && (i%batchSize == 0))
                {
                    beforeUploadHandler(beforeUploadArgs.UpdateData(batch, batchNumber));
                    responseStartIndex = responses.Count;
                    ProcessBatch(batch, responses, methodName);
                    afterUploadHandler(afterUploadArgs.UpdateData(responseStartIndex));
                    ++batchNumber;

                    leftOverCount = 0;
                    batch = new Record[batchSize];
                }
                batch[i%batchSize] = records[i];
                ++leftOverCount;
            }

            Record[] leftOvers  = new Record[leftOverCount];
            Array.Copy(batch, leftOvers, leftOverCount);

            beforeUploadHandler(beforeUploadArgs.UpdateData(leftOvers, batchNumber));
            responseStartIndex = responses.Count;
            ProcessBatch(leftOvers, responses, methodName);
            afterUploadHandler(afterUploadArgs.UpdateData(responseStartIndex));
            return responses.ToArray();
        }

        private WriteResponse[] DirectInvoke(Record[] records, string methodName, int batchSize)
        {
            var beforeUploadArgs = new BeforeBatchUploadEventArgs(methodName, records.Length, batchSize);

            var beforeUploadHandler = GetBeforeUploadHandler();
            var afterUploadHandler = GetAfterUploadHandler();

            beforeUploadHandler(beforeUploadArgs.UpdateData(records, 1));
            var results = InvokeService<WriteResponse[]>(records, methodName);
            afterUploadHandler(new AfterBatchUploadEventArgs(beforeUploadArgs, results).UpdateData(0));
            return results;
        }

        private Action<AfterBatchUploadEventArgs> GetAfterDeleteHandler()
        {
            if (AfterBatchDelete == null)
                return (a) => { }; // Do nothing
            else
            {
                return (args) => AfterBatchDelete(this, args);
            }
        }

        private Action<BeforeBatchDeleteEventArgs> GetBeforeDeleteHandler()
        {
            if (BeforeBatchDelete == null)
                return (a) => { }; // Do nothing
            else
            {
                return (args) => BeforeBatchDelete(this, args);
            }
        }

        private Action<AfterBatchUploadEventArgs> GetAfterUploadHandler()
        {
            if (AfterBatchUpload == null)
                return (a) => { }; // Do nothing
            else
            {
                return (args) => AfterBatchUpload(this, args);
            }
        }

        private Action<BeforeBatchUploadEventArgs> GetBeforeUploadHandler()
        {
            if (BeforeBatchUpload == null)
                return (a) => { }; // Do nothing
            else
            {
                return (args) => BeforeBatchUpload(this, args);
            }
        }

        private void ProcessBatch(Record[] batch, List<WriteResponse> responses, string methodName)
        {
            _log.Debug(string.Format("Processing Batch -- Size: {0} Record(s) , Operation: {1}", batch.Length, methodName));
            try
            {
                responses.AddRange(InvokeService<WriteResponse[]>(batch, methodName));
            }
            catch (Exception e)
            {
                _log.Debug("Processing Batch Error: {0}", e);

                StatusDetail statDetail = new StatusDetail();
                statDetail.code         = StatusDetailCodeType.UNEXPECTED_ERROR;
                statDetail.message      = "Batch threw an Exception";

                Status status = new Status();
                status.isSuccess	= false;
                status.isSuccessSpecified = true;
                status.statusDetail = new StatusDetail[] { statDetail };

                var wr = new WriteResponse() { status = status };
                for (int i = 0; i < batch.Length; i++)
                {
                    responses.Add(wr);
                }
            }
        }

        #endregion

        #region : Search Methods :

        /// <summary>
        /// Searches using the specified search options.
        /// </summary>
        /// <param name="searchRec">The search record.</param>
        /// <returns>Response from the WebService.</returns>
        public override SearchResult Search(SearchRecord searchRec)
        {
            return Search(searchRec, null);
        }

        /// <summary>
        /// Searches the specified search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="returnSearchColumns">
        /// if set to <c>true</c> returns Search Columns rather than entity records.
        /// </param>
        /// <returns>The search result.</returns>
        public override SearchResult Search(SearchRecord search, bool returnSearchColumns)
        {
            SearchPreferences searchPref = Configuration.SearchPreferences.Duplicate();
            searchPref.returnSearchColumns = returnSearchColumns;

            return Search(search, searchPref);
        }

        /// <summary>Executes the specified search.</summary>
        /// <param name="searchRec">The search criteria.</param>
        /// <param name="searchPref">The search preferences.</param>
        public override SearchResult Search(SearchRecord searchRec, SearchPreferences searchPref)
        {
            return InvokeService<SearchResult>(searchRec, "search", searchPref);
        }

        /// <summary>Retrieves the additional pages of records returned for the given search.</summary>
        /// <param name="searchId">The ID of the search.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns>The records pertaining to specified search results page.</returns>
        public SearchResult SearchMoreWithId(string searchId, int pageIndex)
        {
            return SearchMoreWithId(searchId, pageIndex, null);
        }

        /// <summary>Retrieves the additional pages of records returned for the given search.</summary>
        /// <param name="searchId">The ID of the search.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="bodyFieldsOnly">Whether the search should return body fields only.</param>
        /// <returns>The records pertaining to specified search results page.</returns>
        public SearchResult SearchMoreWithId(string searchId, int pageIndex, bool bodyFieldsOnly)
        {
            SearchPreferences searchPref = Configuration.SearchPreferences.Duplicate();
            searchPref.bodyFieldsOnly    = bodyFieldsOnly;

            return SearchMoreWithId(searchId, pageIndex, searchPref);
        }

        /// <summary>Retrieves the additional pages of records returned for the given search.</summary>
        /// <param name="searchId">The ID of the search.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchPrefs">Preference settings for the current search.</param>
        /// <returns>The records pertaining to specified search results page.</returns>
        public SearchResult SearchMoreWithId(string searchId, int pageIndex, SearchPreferences searchPrefs)
        {
            return InvokeService<SearchResult>
                            (
                                new object[] { searchId, pageIndex }, 
                                "searchMoreWithId", 
                                searchPrefs
                            );
        }

        #endregion

        #region : Async Methods :
        
        /// <summary>
        /// Invokes NetSuite's asyncAddList(..) method.
        /// </summary>
        /// <param name="records">The records to be added.</param>
        /// <returns>The filter criteria for the retrieved data.</returns>
        public virtual AsyncStatusResult AsyncAddList(Record[] records)
        {
            return InvokeService<AsyncStatusResult>(records, "asyncAddList");
        }

        /// <summary>
        /// Invokes NetSuite's asyncUpdateList(..) method.
        /// </summary>
        /// <param name="records">The records to be updated.</param>
        /// <returns></returns>
        public virtual AsyncStatusResult AsyncUpdateList(Record[] records)
        {
            return InvokeService<AsyncStatusResult>(records, "asyncUpdateList");
        }

        /// <summary>
        /// Invokes NetSuite's asyncDeleteList(..) method.
        /// </summary>
        /// <param name="baseRefs">The items to be deleted.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual AsyncStatusResult AsyncDeleteList(BaseRef[] baseRefs)
        {
            return InvokeService<AsyncStatusResult>(baseRefs, "asyncDeleteList");
        }

        /// <summary>
        /// Invokes NetSuite's asyncGetList(..) method.
        /// </summary>
        /// <param name="baseRefs">The items to be retrieved.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual AsyncStatusResult AsyncGetList(BaseRef[] baseRefs)
        {
            return InvokeService<AsyncStatusResult>(baseRefs, "asyncGetList");
        }

        /// <summary>
        /// Invokes NetSuite's asyncSearch(..) method.
        /// </summary>
        /// <param name="searchRec">The search options.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual AsyncStatusResult AsyncSearch(SearchRecord searchRec)
        {
            return InvokeService<AsyncStatusResult>(searchRec, "asyncSearch");
        }

        /// <summary>
        /// Invokes NetSuite's getAsyncResult(..) method.
        /// </summary>
        /// <returns>Response from the WebService.</returns>
        public AsyncResult GetAsyncResult(string s, int i)
        {
            return InvokeService<AsyncResult>(new object[] { s, i }, "getAsyncResult");
        }

        /// <summary>
        /// Invokes NetSuite's checkAsyncStatus(..) method.
        /// </summary>
        /// <returns>Response from the WebService.</returns>
        public virtual AsyncStatusResult CheckAsyncStatus(string s)
        {
            return InvokeService<AsyncStatusResult>(s, "checkAsyncStatus");
        }
        #endregion

        #region : Lockdown :

        private  readonly object __locker = new object();

        private  bool __isLockedDown = false;

        public bool IsSuspended
        {
            get
            {
                lock (__locker)
                    return __isLockedDown;
            }
            private set
            {
                lock (__locker)
                {
                    __isLockedDown = value;
                }
            }
        }


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServiceManager"/> class.
        /// </summary>
        public NetSuiteServiceManager() : this(new NullLogger())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServiceManager"/> class.
        /// </summary>
        public NetSuiteServiceManager(ILogger logger) : this(new NetSuiteService(), logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServiceManager"/> class.
        /// </summary>
        /// <param name="service">The service proxy object.</param>
        /// <param name="logger">The logger to be used by the Service Manager.</param>
        public NetSuiteServiceManager(INetSuiteService service, ILogger logger)
        {
            ServiceProxy = service;
            Configuration  = new NetSuiteServiceConfiguration();

            if (logger != null)
                _log = logger;
            else
                _log = new NullLogger();
        }
    }
}
