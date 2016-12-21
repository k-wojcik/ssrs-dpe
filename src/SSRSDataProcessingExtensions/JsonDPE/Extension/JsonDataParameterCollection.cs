using System.Collections;
using Microsoft.ReportingServices.DataProcessing;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonDataParameterCollection : ArrayList, IDataParameterCollection
    {
        public int Add(IDataParameter parameter)
        {
            return base.Add(parameter);
        }
    }
}