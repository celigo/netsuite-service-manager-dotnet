using com.celigo.net.ServiceManager.SuiteTalk;
namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Declares functionality supported by a NetSuite Service Pool.
    /// </summary>
    public interface IServicePool : INetSuiteServiceBase
    {
        /// <summary>
        /// Changes the email or password of the current user.
        /// </summary>
        /// <param name="loginCredentials">The login credentials.</param>
        /// <param name="cpec">The credentials required for the password/email change.</param>
        /// <returns>Response from NetSuite.</returns>
        SessionResponse ChangeEmail(NetSuiteCredential loginCredentials, ChangeEmail cpec);

        /// <summary>
        /// Changes the email or password of the current user.
        /// </summary>
        /// <param name="loginCredentials">The login credentials.</param>
        /// <param name="cpec">The credentials required for the password/email change.</param>
        /// <returns>Response from NetSuite.</returns>
        SessionResponse ChangePassword(NetSuiteCredential loginCredentials, ChangePassword cpec);

        /// <summary>
        /// Retrieves a <see cref="SearchSession"/> token that can be used to retrieve multiple result
        /// pages from a search.
        /// </summary>
        /// <returns></returns>
        SearchSession BeginPaginatedSearch();

        /// <summary>
        /// Searches NetSuite using the specified search parameters.
        /// </summary>
        /// <param name="searchRec">The search parameter.</param>
        /// <param name="prefs">The search preferences.</param>
        /// <returns>Results of the search.</returns>
        SearchResult Search(SearchRecord searchRec, SearchPreferences prefs);

        /// <summary>
        /// Searches NetSuite using the specified search parameters.
        /// </summary>
        /// <param name="searchRec">The search parameter.</param>
        /// <param name="session">A token created using <see cref="BeginPaginatedSearch()"/> method.</param>
        /// <returns>Results of the search.</returns>
        SearchResult Search(SearchRecord searchRec, SearchSession session);

        /// <summary>
        /// Retrieves a page from a Search originally executed using the <paramref name="session"/> token.
        /// </summary>
        /// <param name="pageIndex">Index of the result page to be retrieved.</param>
        /// <param name="session">The search session.</param>
        /// <returns>Results on the specified page.</returns>
        SearchResult SearchMore(int pageIndex, SearchSession session);

        /// <summary>
        /// Retrieves a page from a Search originally executed using the <paramref name="session"/> token.
        /// </summary>
        /// <param name="pageIndex">Index of the result page to be retrieved.</param>
        /// <param name="prefs">Preference settings to be used for the search.</param>
        /// <param name="session">The search session.</param>
        /// <returns>Results on the specified page.</returns>
        SearchResult SearchMore(int pageIndex, SearchPreferences prefs, SearchSession session);

        /// <summary>
        /// Gets or sets the manager for the service pool.
        /// </summary>
        /// <value>The service pool manager.</value>
        NetSuiteServicePoolManager ServicePoolManager { get; set; }
    }
}
