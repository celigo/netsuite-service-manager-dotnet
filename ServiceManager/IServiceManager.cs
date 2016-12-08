using System;
using com.celigo.net.ServiceManager.SuiteTalk;
using com.celigo.net.ServiceManager.Utility;
namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Interface exposed by the Service Manager
    /// </summary>
    public interface IServiceManager: INetSuiteServiceBase
    {
        /// <summary>
        /// Gets or sets the current configuration.
        /// </summary>
        /// <value>The configuration.</value>
        NetSuiteServiceConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the NetSuite credentials.
        /// </summary>
        /// <value>The NetSuite credentials.</value>
        NetSuiteCredential Credentials { get; set; }

        /// <summary>
        /// Changes the email or password of the current user.
        /// </summary>
        /// <param name="cpec">The credentials required for the password/email change.</param>
        /// <returns>Response from NetSuite.</returns>
        SessionResponse ChangeEmail(ChangeEmail cpec);
        /// <summary>
        /// Changes the email or password of the current user.
        /// </summary>
        /// <param name="cpec">The credentials required for the password/email change.</param>
        /// <returns>Response from NetSuite.</returns>
        SessionResponse ChangePassword(ChangePassword cpec);

        /// <summary>
        /// Adopts a session created using the <see cref="INetSuiteServiceBase.CreateSession(NetSuiteCredential)"/>
        /// method.
        /// </summary>
        /// <param name="session">An active session.</param>
        void AdoptSession(IUserSession session);

        /// <summary>
        /// Log out from the current session.
        /// </summary>
        void CloseSession();

        /// <summary>
        /// Searches NetSuite with the given criteria.
        /// </summary>
        /// <param name="searchRec">The search criteria.</param>
        /// <param name="searchPref">Preference settings to be applied to the current Search.</param>
        /// <returns>The Search Result</returns>
        SearchResult Search(SearchRecord searchRec, SearchPreferences searchPref);
        
        /// <summary>
        /// Executes a follow-up Search in order to retrieve an additional results page generated from a previous call to
        /// <see cref="Search(SearchRecord, SearchPreferences)"/>.
        /// </summary>
        /// <param name="searchId">The ID returned by the original search.</param>
        /// <param name="searchPrefs">Preference settings to be applied to the current Search.</param>
        /// <param name="pageIndex">Index of the results page to be retrieved.</param>
        /// <returns>The Search Result</returns>
        SearchResult SearchMoreWithId(string searchId, int pageIndex, SearchPreferences searchPrefs);

        /// <summary>
        /// Executes a follow-up Search in order to retrieve an additional results page generated from a previous call to
        /// <see cref="Search(SearchRecord, SearchPreferences)"/>
        /// </summary>
        /// <param name="searchId">The ID returned by the original search.</param>
        /// <param name="pageIndex">Index of the results page to be retrieved.</param>
        /// <param name="bodyFieldsOnly">Whether to retrieve body fields only.</param>
        /// <returns>The Search Result</returns>
        SearchResult SearchMoreWithId(string searchId, int pageIndex, bool bodyFieldsOnly);

        /// <summary>
        /// Executes a follow-up Search in order to retrieve an additional results page generated from a previous call to
        /// <see cref="Search(SearchRecord, SearchPreferences)"/>
        /// </summary>
        /// <param name="searchId">The ID returned by the original search.</param>
        /// <param name="pageIndex">Index of the results page to be retrieved.</param>
        /// <returns>The Search Result</returns> 
        SearchResult SearchMoreWithId(string searchId, int pageIndex);

        /// <summary>Add the given records asynchronously.</summary>
        /// <param name="records">The records.</param>
        /// <returns>Status of the Async operation.</returns>
        AsyncStatusResult AsyncAddList(Record[] records);
        
        /// <summary>Deletes the given records asynchronously.</summary>
        /// <param name="baseRefs">The records.</param>
        /// <returns>Status of the Async operation.</returns>
        AsyncStatusResult AsyncDeleteList(BaseRef[] baseRefs);

        /// <summary>Gets the given records asynchronously.</summary>
        /// <param name="baseRefs">The record references.</param>
        /// <returns>Status of the Async operation.</returns>
        AsyncStatusResult AsyncGetList(BaseRef[] baseRefs);

        /// <summary>Updates the given records asynchronously.</summary>
        /// <param name="records">The records.</param>
        /// <returns>Status of the Async operation.</returns>
        AsyncStatusResult AsyncUpdateList(Record[] records);

        /// <summary>
        /// Executes the given search asynchronously.
        /// </summary>
        /// <param name="searchRec">The search criteria.</param>
        /// <returns>Status of the Async operation.</returns>
        AsyncStatusResult AsyncSearch(SearchRecord searchRec);

        /// <summary>Checks the status of an async operation.</summary>
        /// <param name="jobId">The job id.</param>
        /// <returns>Status of the Async operation.</returns>
        AsyncStatusResult CheckAsyncStatus(string jobId);

        /// <summary>Gets the result of a async operation.</summary>
        /// <param name="jobId">The job id.</param>
        /// <param name="pageIndex">Index of the results page.</param>
        /// <returns>Result of the Async operation.</returns>
        AsyncResult GetAsyncResult(string jobId, int pageIndex);

        /// <summary>
        /// Occurs before a batch of records is uploaded to NetSuite for AddList, UpdateList or DeleteList
        /// operations.
        /// </summary>
        event EventHandler<BeforeBatchUploadEventArgs> BeforeBatchUpload;

        /// <summary>
        /// Occurs after a batch of records has been uploaded to NetSuite for AddList, UpdateList or DeleteList
        /// operations.
        /// </summary>
        event EventHandler<AfterBatchUploadEventArgs> AfterBatchUpload;

        /// <summary>
        /// Occurs before a batch of records are deleted from NetSuite.
        /// </summary>
        event EventHandler<BeforeBatchDeleteEventArgs> BeforeBatchDelete;

        /// <summary>
        /// Occurs after a batch of records has been deleted from NetSuite.
        /// </summary>
        event EventHandler<AfterBatchUploadEventArgs> AfterBatchDelete;

        /// <summary>
        /// Occurs before Service Manager encounters empty NS Credentials before executing a WS operation.
        /// </summary>
        /// <remarks>
        /// This event provides an opportunity for a caller to provide NS credentials by loading them from
        /// a configuration file or by prompting the user in an interactive application.
        /// </remarks>
        event EventHandler<CredentialsRequiredEventArgs> CredentialsRequired;
    }
}
