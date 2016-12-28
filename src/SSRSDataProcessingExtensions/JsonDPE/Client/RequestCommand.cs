using System.Collections.Generic;

namespace SSRSDataProcessingExtensions.JsonDPE.Client
{
    public class RequestCommand
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string ContentType { get; set; }
        public string Accept { get; set; }
        public bool IsRequestTypeInHeader { get; set; } = true;

        public Dictionary<string, string> HttpHeader { get; set; }

        public object Payload { get; set; }

        public List<HeaderColumn> ColumnHeaders { get; set; }
    }
}