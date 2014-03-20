
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public partial class SearchPreferences
    {
        /// <summary>
        /// Performs a shallow copy of the current instance.
        /// </summary>
        /// <returns>A shallow copy of the current instance.</returns>
        public SearchPreferences Duplicate()
        {
            return (SearchPreferences)this.MemberwiseClone();
        }
    }
}