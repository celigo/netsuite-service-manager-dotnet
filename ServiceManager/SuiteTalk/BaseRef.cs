
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public abstract partial class BaseRef
    {
        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <returns></returns>
        public abstract string GetInternalId();

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Concat(GetType().Name, ": ", name);
        }
    }

    /// <summary>
    /// Reference to a NetSuite data item
    /// </summary>
    public interface IReference
    {
        /// <summary>
        /// Gets or sets the internal id.
        /// </summary>
        /// <value>The internal id.</value>
        string internalId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string name { get; set; }
    }

    public partial class RecordRef : IReference
    {
        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <returns></returns>
        public override string GetInternalId()
        {
            return this.internalId;
        }

        /// <summary>
        /// Returns the name of the current record.
        /// </summary>
        public override string ToString()
        {
            return string.Concat(internalId, " ", name);
        }
    }

    public partial class CustomRecordRef : IReference
    {
        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <returns></returns>
        public override string GetInternalId()
        {
            return this.internalId;
        }
    }

    public partial class CustomizationRef : IReference
    {
        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <returns></returns>
        public override string GetInternalId()
        {
            return this.internalId;
        }
    }

    public partial class InitializeRef : IReference
    {
        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <returns></returns>
        public override string GetInternalId()
        {
            return this.internalId;
        }
    }

    public partial class InitializeAuxRef : IReference
    {
        /// <summary>
        /// Gets the internal id.
        /// </summary>
        /// <returns></returns>
        public override string GetInternalId()
        {
            return this.internalId;
        }
    }

    public partial class ListOrRecordRef : IReference
    {
    }
}
