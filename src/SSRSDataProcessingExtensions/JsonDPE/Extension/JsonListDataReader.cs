using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ReportingServices.DataProcessing;
using SSRSDataProcessingExtensions.JsonDPE.Client;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonListDataReader : IDataReader
    {
        private readonly WebServiceListResponse _listResponse;
        private readonly IEnumerator<List<object>> _enumerator;

        public JsonListDataReader(WebServiceListResponse listResponse)
        {
            _listResponse = listResponse;
            _enumerator = _listResponse.Data.GetEnumerator();
        }

        public int FieldCount
        {
            get { return _listResponse.ColumnHeaders.Count; }
        }

        public void Dispose()
        {
        }

        public Type GetFieldType(int fieldIndex)
        {
            return Type.GetType(_listResponse.ColumnHeaders.Single(x => x.Ordinal == fieldIndex).Type);
        }

        public string GetName(int fieldIndex)
        {
            return _listResponse.ColumnHeaders.Single(x=>x.Ordinal == fieldIndex).Name;
        }

        public int GetOrdinal(string fieldName)
        {
            return _listResponse.ColumnHeaders.Single(x=>x.Name == fieldName).Ordinal;
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