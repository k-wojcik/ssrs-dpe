using Microsoft.ReportingServices.DataProcessing;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonDataParameter : IDataParameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }
}