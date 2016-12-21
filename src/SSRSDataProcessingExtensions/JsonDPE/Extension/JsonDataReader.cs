using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ReportingServices.DataProcessing;
using SSRSDataProcessingExtensions.JsonDPE.Client;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonDataReader : IDataReader
    {
        private readonly WebServiceResponse _response;
        private IEnumerator<List<object>> _enumerator;

        public JsonDataReader(WebServiceResponse response)
        {
            _response = response;
            _enumerator = _response.Data.GetEnumerator();
        }

        public int FieldCount
        {
            get { return _response.ColumnHeaders.Count; }
        }

        public void Dispose()
        {
        }

        public Type GetFieldType(int fieldIndex)
        {
            return Type.GetType(_response.ColumnHeaders[fieldIndex].Type);
        }

        public string GetName(int fieldIndex)
        {
            return _response.ColumnHeaders[fieldIndex].Name;
        }

        public int GetOrdinal(string fieldName)
        {
            return _response.ColumnHeaders.Single(x=>x.Name == fieldName).Ordinal;
        }

        public object GetValue(int fieldIndex)
        {
            return _enumerator.Current[fieldIndex];
        }

        public bool Read()
        {
            return _enumerator.MoveNext();
        }
    }
}