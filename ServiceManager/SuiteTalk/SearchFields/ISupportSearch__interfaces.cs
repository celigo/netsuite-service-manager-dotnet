
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    /// <summary>
    /// Represents a class that supports NetSuite Search Operator
    /// </summary>
    /// <typeparam name="T">The enum type of the Search Operator supported</typeparam>
    public interface ISupportSearchOperator<T>
    {
        /// <summary>
        /// Gets or sets the search operator.
        /// </summary>
        /// <value>The operator.</value>
        T @operator { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the search operator was specified.
        /// </summary>
        /// <value><c>true</c> if the search operator was specified; otherwise, <c>false</c>.</value>
        bool operatorSpecified { get; set; }
    }

    /// <summary>
    /// Represents a class that holds a Search Value
    /// </summary>
    /// <typeparam name="T">The type of the Search Value</typeparam>
    public interface ISupportSearchValue<T>
    {
        /// <summary>
        /// Gets the search value.
        /// </summary>
        /// <returns></returns>
        T GetSearchValue();

        /// <summary>
        /// Sets the search value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>Sets the searchValueSpecified field if applicable</remarks>
        void SetSearchValue(T value);
    }

    /// <summary>
    /// Indicates that the search field supports reference type (e.g. RecordRef)
    /// search values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISupportReferenceSearchValue<T>
    {
        /// <summary>
        /// Gets the search value.
        /// </summary>
        /// <returns></returns>
        T GetSearchValue();
    }

    /// <summary>
    /// Represents a class that holds a secondary Search Value
    /// </summary>
    /// <typeparam name="T">The type of the Search Value</typeparam>
    public interface ISupportSearchValue2<T>
    {
        /// <summary>
        /// Gets the search value.
        /// </summary>
        /// <returns></returns>
        T GetSearchValue2();

        /// <summary>
        /// Sets the search value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>Sets the searchValueSpecified field if applicable</remarks>
        void SetSearchValue2(T value);
    }
}
