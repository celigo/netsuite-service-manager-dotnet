namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public partial class CustomFieldRef 
    {
        /// <summary>
        /// Gets internal id of the custom field.
        /// </summary>
        /// <value>The internal id.</value>
        public abstract string GetInternalId();

        /// <summary>
        /// Sets the internal id of the custom field..
        /// </summary>
        /// <param name="id">The id.</param>
        public abstract void SetInternalId(string id);
    }
}
