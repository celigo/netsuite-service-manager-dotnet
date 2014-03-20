using System;
using com.celigo.net.ServiceManager.SuiteTalk;
using com.celigo.net.ServiceManager.Utility;

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Base interface for all NetSuite service classes
    /// </summary>
    public interface INetSuiteServiceBase
    {
        /// <summary>
        /// Occurs before invoking a NS WS API method.
        /// </summary>
        event EventHandler<ServiceInvocationEventArgs> BeforeServiceInvocation;

        /// <summary>
        /// Occurs when after a WS API method is invoked.
        /// </summary>
        event EventHandler<ServiceInvocationEventArgs> AfterServiceInvocation;

        /// <summary>
        /// Occurs when a WS API method throws an error.
        /// </summary>
        event EventHandler<ServiceInvocationEventArgs> ServiceInvocationError;

        /// <summary>Logout from the specified session.</summary>
        /// <param name="session">The active session.</param>
        void CloseSession(IUserSession session);

        /// <summary>Logins in to NetSuite using the Credentials provided.</summary>
        /// <param name="credential">NetSuite user's Credentials.</param>
        /// <returns>Response from NetSuite.</returns>
        IUserSession CreateSession(NetSuiteCredential credential);

        /// <summary>
        /// Gets the server time.
        /// </summary>
        /// <returns></returns>
        GetServerTimeResult GetServerTime();
        /// <summary>
        /// Adds the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        WriteResponse Add(Record record);
        /// <summary>
        /// Adds the list of records.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        WriteResponse[] AddList(Record[] records);
        /// <summary>
        /// Attaches the specified records.
        /// </summary>
        /// <param name="attachRef">The attachment information.</param>
        /// <returns></returns>
        WriteResponse Attach(AttachReference attachRef);
        /// <summary>
        /// Deletes the specified record.
        /// </summary>
        /// <param name="baseRef">The record reference.</param>
        /// <returns></returns>
        WriteResponse Delete(BaseRef baseRef);
        /// <summary>
        /// Deletes the list of records.
        /// </summary>
        /// <param name="baseRefs">The record references.</param>
        /// <returns></returns>
        WriteResponse[] DeleteList(BaseRef[] baseRefs);
        /// <summary>
        /// Detaches the specified attachment.
        /// </summary>
        /// <param name="attachRef">The attachment info.</param>
        /// <returns></returns>
        WriteResponse Detach(AttachReference attachRef);
        /// <summary>
        /// Gets the specified record.
        /// </summary>
        /// <param name="baseRef">The record reference.</param>
        /// <returns></returns>
        ReadResponse Get(BaseRef baseRef);
        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <param name="gar">The record types to get.</param>
        /// <returns></returns>
        GetAllResult GetAll(GetAllRecord gar);
        /// <summary>
        /// Gets the customizations of specified type.
        /// </summary>
        /// <param name="customization">The customization type.</param>
        /// <returns></returns>
        GetCustomizationIdResult GetCustomizationId(CustomizationType customization);
        /// <summary>
        /// Gets the deleted records.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        GetDeletedResult GetDeleted(GetDeletedFilter filter);
        /// <summary>
        /// Gets the list or records.
        /// </summary>
        /// <param name="baseRefs">The record references.</param>
        /// <returns></returns>
        ReadResponse[] GetList(BaseRef[] baseRefs);
        /// <summary>
        /// Use to emulate the UI workflow by pre-populating fields on transaction line items with 
        /// values from a related record.
        /// </summary>
        /// <param name="record">Information about the transaction to initialize.</param>
        /// <returns></returns>
        ReadResponse Initialize(InitializeRecord record);

        /// <summary>Searches NetSuite using the specified criteria.</summary>
        /// <param name="record">The search criteria.</param>
        /// <param name="returnSearchColumns">
        /// if set to <c>true</c> returns Search Columns rather than entity records.
        /// </param>
        /// <returns>The search result.</returns>
        SearchResult Search(SearchRecord record);
        /// <summary>Searches NetSuite using the specified criteria.</summary>
        /// <param name="record">The search criteria.</param>
        /// <param name="returnSearchColumns">
        /// if set to <c>true</c> returns Search Columns rather than entity records.
        /// </param>
        /// <returns>The search result.</returns>
        SearchResult Search(SearchRecord record, bool returnSearchColumns);
        /// <summary>
        /// Updates the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        WriteResponse Update(Record record);
        /// <summary>
        /// Updates the list.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        WriteResponse[] UpdateList(Record[] records);
        /// <summary>
        /// Gets the saved search.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        GetSavedSearchResult GetSavedSearch(GetSavedSearchRecord record);
        /// <summary>
        /// Gets the select value.
        /// </summary>
        /// <param name="fieldDescription">The field description.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns></returns>
        GetSelectValueResult GetSelectValue(GetSelectValueFieldDescription fieldDescription, int pageIndex);
        /// <summary>
        /// Gets the item availability.
        /// </summary>
        /// <param name="iaf">The filter.</param>
        /// <returns></returns>
        GetItemAvailabilityResult GetItemAvailability(ItemAvailabilityFilter iaf);
        /// <summary>
        /// Gets the posting transaction summary.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns></returns>
        GetPostingTransactionSummaryResult GetPostingTransactionSummary(PostingTransactionSummaryField fields, com.celigo.net.ServiceManager.SuiteTalk.PostingTransactionSummaryFilter filters, int pageIndex);
    }
}
