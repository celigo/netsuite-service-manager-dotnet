
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    /// <summary>
    /// Represents an Advacned Search record.
    /// </summary>
    public interface ISearchAdvanced
    {
        /// <summary>Gets the criteria for the Search.</summary>
        /// <returns>The criteria for the Search.</returns>
        ISearchRecord GetCriteria();

        /// <summary>
        /// Gets the criteria for the Search.
        /// </summary>
        /// <param name="create">if set to <c>true</c> create the criteria if it is null.</param>
        /// <returns>The criteria for the Search.</returns>
        ISearchRecord GetCriteria(bool create);

        /// <summary>
        /// Sets the criteria.
        /// </summary>
        /// <param name="search">The search.</param>
        void SetCriteria(ISearchRecord search);

        /// <summary>Gets the columns to be returned in the results.</summary>
        /// <returns>The columns to be returned in the results.</returns>
        SearchRow GetColumns();

        /// <summary>
        /// Gets the columns to be returned in the results.
        /// </summary>
        /// <param name="create">if set to <c>true</c> create the columns object if it is null.</param>
        /// <returns>
        /// The columns to be returned in the results.
        /// </returns>
        SearchRow GetColumns(bool create);

        /// <summary>
        /// Sets the return columns.
        /// </summary>
        void SetColumns(SearchRow columns);

        /// <summary>
        /// Gets the saved search id.
        /// </summary>
        /// <returns>The saved search ID.</returns>
        string GetSavedSearchId();
        /// <summary>
        /// Sets the saved search id.
        /// </summary>
        /// <param name="savedSearchId">The saved search id.</param>
        void SetSavedSearchId(string savedSearchId);

        /// <summary>
        /// Gets the saved search script id.
        /// </summary>
        /// <returns></returns>
        string GetSavedSearchScriptId();
        /// <summary>
        /// Sets the saved search script id.
        /// </summary>
        /// <param name="savedSearchScriptId">The saved search script id.</param>
        void SetSavedSearchScriptId(string savedSearchScriptId);
    }
}
