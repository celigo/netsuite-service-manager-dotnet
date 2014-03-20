using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Holds data regarding a BeforeBatchDelete event.
    /// </summary>
    public class BeforeBatchDeleteEventArgs : BeforeBatchProcessEventArgs
    {
        /// <summary>
        /// Gets the references to records that will be deleted.
        /// </summary>
        public BaseRef[] Records { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeBatchDeleteEventArgs"/> class.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="totalRecords">The total number of records.</param>
        /// <param name="batchSize">Size of the batch.</param>
        public BeforeBatchDeleteEventArgs(string methodName, int totalRecords, int batchSize) 
            : base(methodName, totalRecords, batchSize)
        {
        }

        internal BeforeBatchDeleteEventArgs UpdateData(BaseRef[] batch, int batchNumber)
        {
            Records = batch;
            base.BatchNumber = batchNumber;
            base.CurrentBatchSize = batch.Length;
            return this;
        }
    }
}
