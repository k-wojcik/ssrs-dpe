using System;
using Microsoft.ReportingServices.DataProcessing;
using SSRSDataProcessingExtensions.JsonDPE.Client;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonConnection : IDbConnection
    {
        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return 0; }
        }

        public string LocalizedName
        {
            get { return "REST JSON"; }
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
        }

        public IDbCommand CreateCommand()
        {
            return new JsonCommand(new RestClient(_connectionString));
        }

        public void Dispose()
        {
        }

        public void Open()
        {
        }

        public void SetConfiguration(string configuration)
        {
        }
    }
}