using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.celigo.net.ServiceManager.SuiteTalk
{
    partial class SiteCategory1
    {
        public override string GetInternalId()
        {
            return this.internalId;
        }

        public override void SetInternalId(string id)
        {
            this.internalId = id;
        }
    }

    partial class ContactRole
    {
        public override string GetInternalId()
        {
            return this.internalId;
        }

        public override void SetInternalId(string id)
        {
            this.internalId = id;
        }
    }
}
