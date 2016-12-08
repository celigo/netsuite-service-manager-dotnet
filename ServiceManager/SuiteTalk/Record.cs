
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    /// <summary>
    /// Represents a record in NetSuite.
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// Gets or sets the internal id.
        /// </summary>
        /// <value>The internal id.</value>
        string internalId { get; set; }
    }

    public partial class Record
    {
#if !FIRSTBUILD
        /// <summary>
        /// Gets the Internal ID.
        /// </summary>
        /// <returns>The Internal ID of the record.</returns>
        public abstract string GetInternalId();

        /// <summary>
        /// Sets the Internal ID.
        /// </summary>
        /// <param name="id">The Internal ID.</param>
        public abstract void SetInternalId(string id);
#endif
    }
}
