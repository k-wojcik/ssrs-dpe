using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SSRSDataProcessingExtensions.JsonDPE.Client
{
    public class RestClient
    {
        private string _baseUrl;

        public RestClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public WebServiceResponse ExecuteRequest(RequestCommand request)
        {
            var response = new WebServiceResponse();

            var webRequest = (HttpWebRequest)WebRequest.Create($"{_baseUrl}/{request.Path}");
            webRequest.ContentType = !string.IsNullOrEmpty(request.ContentType) ? request.ContentType : "application/json";
            webRequest.Accept = !string.IsNullOrEmpty(request.Accept) ? request.Accept : "application/json";
            webRequest.Method = !string.IsNullOrEmpty(request.Method) ? request.Method : "GET";

            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(JsonConvert.SerializeObject(request.Payload));
            }

            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                response = JsonConvert.DeserializeObject<WebServiceResponse>(responseText);
            }

            return response;
        }
    }
}