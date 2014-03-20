
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    /// <summary>
    /// Represents records that support Custom Fields.
    /// </summary>
    public interface ISupportsCustomFields<T>
    {
        /// <summary>
        /// Gets or sets the custom field list.
        /// </summary>
        /// <value>The custom field list.</value>
        T[] customFieldList { get; set; }

        /// <summary>
        /// Gets custom field with the given ID or <c>null</c> if a field with the
        /// given ID was not found.
        /// </summary>
        K FindCustomField<K>(string fieldId) where K : T;

        /// <summary>
        /// Gets custom field with the given ID or <c>null</c> if a field with the
        /// given ID was not found.
        /// </summary>
        T FindCustomField(string fieldId);
    }
}
