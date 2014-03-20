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
        /// Invokes NetSuite's add(..) method.
        /// </summary>
        /// <param name="record">The record to be added.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual WriteResponse Add(Record record)
        {
            return InvokeService<WriteResponse>(record, "add");
        }

        /// <summary>
        /// Invokes NetSuite's initialize(..) method.
        /// </summary>
        /// <param name="record">The record to be initialized.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual ReadResponse Initialize(InitializeRecord record)
        {
            return InvokeService<ReadResponse>(record, "initialize");
        }

        /// <summary>
        /// Invokes NetSuite's attach(..) method.
        /// </summary>
        /// <param name="attachRef">The item to be attached.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual WriteResponse Attach(AttachReference attachRef)
        {
            return InvokeService<WriteResponse>(attachRef, "attach");
        }

        /// <summary>
        /// Invokes NetSuite's detach(..) method.
        /// </summary>
        /// <param name="attachRef">The attach ref.</param>
        /// <returns></returns>
        public virtual WriteResponse Detach(AttachReference attachRef)
        {
            return InvokeService<WriteResponse>(attachRef, "detach");
        }

        /// <summary>
        /// Invokes NetSuite's addList(..) method.
        /// </summary>
        /// <param name="records">The records to be added.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual WriteResponse[] AddList(Record[] records)
        {
            if (records == null || records.Length == 0)
                return new WriteResponse[0];
            return InvokeService<WriteResponse[]>(records, "addList");
        }

        /// <summary>
        /// Invokes NetSuite's delete(..) method.
        /// </summary>
        /// <param name="baseRef">The item to be deleted.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual WriteResponse Delete(BaseRef baseRef)
        {
            return InvokeService<WriteResponse>(baseRef, "delete");
        }

        /// <summary>
        /// Invokes NetSuite's deleteList(..) method.
        /// </summary>
        /// <param name="baseRefs">The items to be deleted.</param>
        /// <returns>Invokes NetSuite's add(..) method.</returns>
        public virtual WriteResponse[] DeleteList(BaseRef[] baseRefs)
        {
            if (baseRefs == null || baseRefs.Length == 0)
                return new WriteResponse[0];
            return InvokeService<WriteResponse[]>(baseRefs, "deleteList");
        }

        /// <summary>
        /// Invokes NetSuite's getList(..) method.
        /// </summary>
        /// <param name="baseRefs">The items to be retrieved.</param>
        /// <returns>Invokes NetSuite's add(..) method.</returns>
        public virtual ReadResponse[] GetList(BaseRef[] baseRefs)
        {
            if (baseRefs == null || baseRefs.Length == 0)
                return new ReadResponse[0];
            return InvokeService<ReadResponse[]>(baseRefs, "getList");
        }

        /// <summary>
        /// Invokes NetSuite's getDeleted(..) method..
        /// </summary>
        /// <param name="filter">The filter criteria for the retrieved data.</param>
        /// <returns>Invokes NetSuite's add(..) method.</returns>
        public virtual GetDeletedResult GetDeleted(GetDeletedFilter filter)
        {
            return InvokeService<GetDeletedResult>(filter, "getDeleted");
        }

        /// <summary>Retrieves a list of existing saved search IDs on a per-record-type basis.</summary>
        /// <param name="record">
        /// Contains an array of record objects. The record type is an abstract type so an instance of a 
        /// type that extends record must be used—such as Customer or Event.
        /// </param>
        /// <returns></returns>
        public virtual GetSavedSearchResult GetSavedSearch(GetSavedSearchRecord record)
        {
            return InvokeService<GetSavedSearchResult>(record, "getSavedSearch");
        }

        /// <summary>
        /// Use to get server time, resulting in more accurate and reliable sync'ing of data 
        /// than using using local client time. The client will use the time from server to determine 
        /// if the record has changed since the last synchronization.
        /// </summary>
        /// <returns></returns>
        public virtual GetServerTimeResult GetServerTime()
        {
            return InvokeService<GetServerTimeResult>(null, "getServerTime");
        }

        /// <summary>Retrieves a summary of the actual data that posted to the general ledger in an account.</summary>
        /// <param name="fields">Specifies how the data should be grouped.</param>
        /// <param name="filters">Specify the filtering criteria.</param>
        /// <param name="pageIndex">Specify the page to be returned.</param>
        /// <returns></returns>
        public virtual GetPostingTransactionSummaryResult GetPostingTransactionSummary
                                                                (
                                                                    PostingTransactionSummaryField fields,
                                                                    PostingTransactionSummaryFilter filters,
                                                                    int pageIndex
                                                                )
        {
            return InvokeService<GetPostingTransactionSummaryResult>(new object[]
                                                                        {
                                                                            fields,
                                                                            filters,
                                                                            pageIndex,
                                                                        },
                                                                        "getPostingTransactionSummary");
        }

        /// <summary>
        /// Invokes NetSuite's update(..) method.
        /// </summary>
        /// <param name="record">The record to be updated.</param>
        /// <returns>Invokes NetSuite's add(..) method.</returns>
        public virtual WriteResponse Update(Record record)
        {
            return InvokeService<WriteResponse>(record, "update");
        }

        /// <summary>
        /// Invokes NetSuite's updateList(..) method.
        /// </summary>
        /// <param name="records">The records to be updated.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual WriteResponse[] UpdateList(Record[] records)
        {
            if (records == null || records.Length == 0)
                return new WriteResponse[0];
            return InvokeService<WriteResponse[]>(records, "updateList");
        }

        /// <summary>
        /// Invokes NetSuite's get(..) method.
        /// </summary>
        /// <param name="baseRef">The item to be retrieved.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual ReadResponse Get(BaseRef baseRef)
        {
            return InvokeService<ReadResponse>(baseRef, "get");
        }

        /// <summary>
        /// Invokes NetSuite's getAll(..) method.
        /// </summary>
        /// <param name="gar">The filter criteria for the retrieved data.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual GetAllResult GetAll(GetAllRecord gar)
        {
            return InvokeService<GetAllResult>(gar, "getAll");
        }

        /// <summary>
        /// Invokes NetSuite's getSelectValue(..) method.
        /// </summary>
        /// <param name="fieldDescription">The field description.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns></returns>
        public virtual GetSelectValueResult GetSelectValue(
                                    GetSelectValueFieldDescription fieldDescription,
                                    int pageIndex)
        {
            return InvokeService<GetSelectValueResult>(new object[]
                                                            {
                                                                fieldDescription,
                                                                pageIndex,
                                                            },
                                                            "getSelectValue");
        }
        
        /// <summary>
        /// Use to retrieve the internalIds, externalIds, and/or scriptIds of all custom objects of a specified type.
        /// </summary>
        /// <param name="customization">Any of the custom object types enumerated in <see cref="CustomizationType"/></param>
        /// <returns>Amongst other information, A list of custom objects that correspond to the specified customization type. 
        /// Also returns the internalId, externalId, and/or scriptId of each object.</returns>
        public virtual GetCustomizationIdResult GetCustomizationId(CustomizationType customization)
        {
            return InvokeService<GetCustomizationIdResult>(new object[] { customization, false }, "getCustomizationId");
        }

        /// <summary>
        /// Invokes NetSuite's getItemAvailability(..) method.
        /// </summary>
        /// <param name="iaf">The filter criteria for the retrieved data.</param>
        /// <returns>Response from the WebService.</returns>
        public virtual GetItemAvailabilityResult GetItemAvailability(ItemAvailabilityFilter iaf)
        {
            return InvokeService<GetItemAvailabilityResult>(iaf, "getItemAvailability");
        }

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
