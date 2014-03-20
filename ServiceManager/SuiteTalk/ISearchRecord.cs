
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    /// <summary>
    /// Represents basic criteria for a NetSuite search.
    /// </summary>
    public interface ISearchBasic { }

    /// <summary>
    /// Represents a NetSuite Search 
    /// </summary>
    public interface ISearchRecord
    {
        /// <summary>
        /// Gets the basic search criteria.
        /// </summary>
        /// <returns>The basic search criteria</returns>
        ISearchBasic GetSearchBasic();

        /// <summary>
        /// Gets the basic search criteria.
        /// </summary>
        /// <param name="create">if set to <c>true</c> creates the basic criteria if null.</param>
        /// <returns>The basic search criteria</returns>
        ISearchBasic GetSearchBasic(bool create);
    }
}
