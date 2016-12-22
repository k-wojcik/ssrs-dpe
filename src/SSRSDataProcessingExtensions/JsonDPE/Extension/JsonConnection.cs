using System;
using Microsoft.ReportingServices.DataProcessing;
using SSRSDataProcessingExtensions.JsonDPE.Client;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonConnection : IDbConnectionExtension
    {
        private string _connectionString;
        private string _user;
        private string _password;
        private bool _integratedSecurity;
        private string _impersonate;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return 0; }
        }

        public string Impersonate
        {
            set { _impersonate = value; }
        }

        public bool IntegratedSecurity
        {
            get { return _integratedSecurity; }
            set { _integratedSecurity = value; }
        }

        public string LocalizedName
        {
            get { return "REST JSON"; }
        }

        public string Password
        {
            set { _password = value; }
        }

        public string UserName
        {
            set { _user = value; }
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