using System;
using System.Collections.Generic;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Holds data associated with a AfterBatchUpload event.
    /// </summary>
    public class AfterBatchUploadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the information about the last upload.
        /// </summary>
        public BeforeBatchProcessEventArgs UploadArgs { get; private set; }

        /// <summary>
        /// Gets the responses for the last update.
        /// </summary>
        public IEnumerable<WriteResponse> Responses
        {
            get
            {
                for (int i = _startIndex; i < _responses.Count; i++)
                    yield return _responses[i];
            }
        }

        private readonly IList<WriteResponse> _responses;
        private int _startIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterBatchUploadEventArgs"/> class.
        /// </summary>
        /// <param name="beforeUploadArgs">The <see cref="com.celigo.net.ServiceManager.Utility.BeforeBatchProcessEventArgs"/> instance containing the event data.</param>
        /// <param name="responses">The responses.</param>
        public AfterBatchUploadEventArgs(BeforeBatchProcessEventArgs beforeUploadArgs, IList<WriteResponse> responses)
        {
            this.UploadArgs = beforeUploadArgs;
            this._responses = responses;
        }

        internal AfterBatchUploadEventArgs UpdateData(int startIndex)
        {
            this._startIndex = startIndex;
            return this;
        }
    }
}
