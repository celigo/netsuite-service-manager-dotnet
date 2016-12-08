#pragma warning disable 1591
using System;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager
{
    public partial class NetSuiteServiceManager
    {
#if !FIRSTBUILD
        public override ReadResponse[] GetList(BaseRef[] records)
        {
            if (records == null || records.Length == 0)
                return new ReadResponse[0];
            return InvokeService<ReadResponseList>(records, "getList").readResponse;
        }

        public override ReadResponse[] InitializeList(InitializeRecord[] records)
        {
            if (records == null || records.Length == 0)
                return new ReadResponse[0];
            return InvokeService<ReadResponseList>(records, "initializeList").readResponse;
        }
        
#endif
    }
}