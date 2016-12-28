using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ReportingServices.DataProcessing;
using Newtonsoft.Json;
using SSRSDataProcessingExtensions.JsonDPE.Client;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonCommand : IDbCommand
    {
        private readonly RestClient _restClient;
        private JsonDataParameterCollection _parameters;
        private string _commandText;
        private int _timeout = 0;

        public JsonCommand(RestClient restClient)
        {
            _restClient = restClient;
            _parameters = new JsonDataParameterCollection();
        }

        public string CommandText
        {
            get { return _commandText; }
            set
            {
                _commandText = value;   
            }
        }

        public int CommandTimeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public CommandType CommandType
        {
            get { return CommandType.Text;}
            set { if (value != CommandType.Text) throw new NotSupportedException(); }
        }

        public IDataParameterCollection Parameters
        {
            get { return _parameters; }
        }

        public IDbTransaction Transaction
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Cancel()
        {
            throw new NotSupportedException();
        }

        public IDataParameter CreateParameter()
        {
           return new JsonDataParameter();
        }

        public void Dispose()
        {
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            RequestCommand request = JsonConvert.DeserializeObject<RequestCommand>(ReplaceParameters(_commandText, _parameters));
            if (request.PropertyMap != null)
            {
                return new JsonListDataReader(_restClient.ExecuteRequest<WebServiceListResponse>(request, behavior.ToString()));
            }
            else
            {
                return new JsonMapperDataReader(request, _restClient.ExecuteRequest<List<object>>(request, behavior.ToString()));
            }   
        }

        private string ReplaceParameters(string command, JsonDataParameterCollection parameters)
        {
            foreach (IDataParameter parameter in parameters)
            {
                command = command.Replace(parameter.ParameterName, JsonConvert.SerializeObject(parameter.Value));
            }

            return command;
        }
    }
}