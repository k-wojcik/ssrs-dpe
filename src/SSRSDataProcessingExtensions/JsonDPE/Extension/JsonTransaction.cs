using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ReportingServices.DataProcessing;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonTransaction : IDbTransaction
    {
        public void Dispose()
        {
            throw new NotSupportedException();
        }

        public void Commit()
        {
            throw new NotSupportedException();
        }

        public void Rollback()
        {
            throw new NotSupportedException();
        }
    }
}
