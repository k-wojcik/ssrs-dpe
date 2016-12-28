using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.ReportingServices.DataProcessing;
using Newtonsoft.Json;
using SSRSDataProcessingExtensions.JsonDPE.Extension;

namespace SSRSDataProcessingExtensions.JsonDPE.Client
{
    public class RestClient
    {
        private string _baseUrl;

        public RestClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public T ExecuteRequest<T>(RequestCommand request, string requestType)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create($"{_baseUrl}/{request.Path}");
            webRequest.ContentType = !string.IsNullOrEmpty(request.ContentType) ? request.ContentType : "application/json";
            webRequest.Accept = !string.IsNullOrEmpty(request.Accept) ? request.Accept : "application/json";
            webRequest.Method = !string.IsNullOrEmpty(request.Method) ? request.Method : "GET";

            if (request.HttpHeader != null)
            {
                foreach (var headerItem in request.HttpHeader)
                {
                    webRequest.Headers.Add(headerItem.Key, headerItem.Value);
                }
            }
            
            if (request.IsRequestTypeInHeader)
            {
                webRequest.Headers.Add("ssrs-request-type", requestType);
            }

            if (webRequest.Method != "GET" && webRequest.Method != "HEAD")
            {
                using (var writer = new StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(JsonConvert.SerializeObject(request.Payload));
                }
            }     

            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(responseText);
            }
        }
    }
}