#if !FIRSTBUILD
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public interface ISearchRow
    {
        /// <summary>Gets the Search Row Basic value.</summary>
        /// <returns>The ISearchRowBasic assigned to this row.</returns>
        ISearchRowBasic GetSearchRowBasic();

        /// <summary>
        /// Sets the Search Row Basic value.
        /// </summary>
        /// <param name="basic">The ISearchRowBasic to be assigned to this row.</param>
        void SetSearchRowBasic(ISearchRowBasic basic);
    }
}
#endif
