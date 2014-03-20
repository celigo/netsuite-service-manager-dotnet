using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Holds data regarding a BeforeBatchUploadEvent.
    /// </summary>
    public class BeforeBatchUploadEventArgs : BeforeBatchProcessEventArgs
    {
        /// <summary>
        /// Gets the batch of records to be uploaded.
        /// </summary>
        public Record[] Batch { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeBatchUploadEventArgs"/> class.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="totalRecords">The total number of records.</param>
        /// <param name="batchSize">Size of the batch.</param>
        public BeforeBatchUploadEventArgs(string methodName, int totalRecords, int batchSize) 
            : base(methodName, totalRecords, batchSize)
        {
        }

        internal BeforeBatchUploadEventArgs UpdateData(Record[] batch, int batchNumber)
        {
            this.Batch       = batch;
            this.BatchNumber = batchNumber;
            this.CurrentBatchSize = batch.Length;
            return this;
        }
    }
}
