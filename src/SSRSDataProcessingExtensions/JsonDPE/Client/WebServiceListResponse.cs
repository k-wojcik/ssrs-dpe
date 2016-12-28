using System.Collections.Generic;

namespace SSRSDataProcessingExtensions.JsonDPE.Client
{
    public class WebServiceListResponse
    {
        public List<List<object>> Data { get; set; }
        public List<ResponseHeaderColumn> ColumnHeaders { get; set; }
    }
}