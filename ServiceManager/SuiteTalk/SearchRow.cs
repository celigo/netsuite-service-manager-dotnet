
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public partial class SearchRow
    {
        /// <summary>Gets the Search Row Basic value.</summary>
        /// <returns>The ISearchRowBasic assigned to this row.</returns>
        public abstract ISearchRowBasic GetSearchRowBasic();

        /// <summary>
        /// Sets the Search Row Basic value.
        /// </summary>
        /// <param name="basic">The ISearchRowBasic to be assigned to this row.</param>
        public abstract void SetSearchRowBasic(ISearchRowBasic basic);
    }
}
