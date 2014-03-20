using System;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Holds data regarding a BeforeBatchProcess event.
    /// </summary>
    public class BeforeBatchProcessEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; private set; }
        /// <summary>
        /// Gets the total records.
        /// </summary>
        /// <value>
        /// The total number of records.
        /// </value>
        public int TotalRecords { get; private set; }
        /// <summary>
        /// Gets the size of the batch.
        /// </summary>
        /// <value>
        /// The size of the batch.
        /// </value>
        public int BatchSize { get; private set; }
        /// <summary>
        /// Gets the number of records in the current batch.
        /// </summary>
        /// <value>
        /// The size of the current batch.
        /// </value>
        public int CurrentBatchSize { get; protected set; }
        /// <summary>
        /// Gets the batch number.
        /// </summary>
        /// <value>
        /// The batch number.
        /// </value>
        public int BatchNumber { get; protected internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeBatchProcessEventArgs"/> class.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="totalRecords">The total number of records.</param>
        /// <param name="batchSize">Size of the batch.</param>
        public BeforeBatchProcessEventArgs (string methodName, int totalRecords, int batchSize)
        {
            this.MethodName = methodName;
            this.TotalRecords = totalRecords;
            this.BatchSize = batchSize;
        }
    }
}
