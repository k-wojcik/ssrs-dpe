using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ReportingServices.DataProcessing;
using SSRSDataProcessingExtensions.JsonDPE.Client;

namespace SSRSDataProcessingExtensions.JsonDPE.Extension
{
    public class JsonMapperDataReader : IDataReader
    {
        private RequestCommand _request;
        private readonly List<object> _listResponse;
        private readonly IEnumerator<object> _enumerator;

        public JsonMapperDataReader(RequestCommand request, List<object> listResponse)
        {
            _request = request;
            _listResponse = listResponse;
            _enumerator = _listResponse.GetEnumerator();
        }

        public int FieldCount
        {
            get { return _request.ColumnHeaders.Count; }
        }

        public void Dispose()
        {
        }

        public Type GetFieldType(int fieldIndex)
        {
            return Type.GetType(_request.ColumnHeaders.Single(x => x.Ordinal == fieldIndex).Type);
        }

        public string GetName(int fieldIndex)
        {
            return _request.ColumnHeaders.Single(x=>x.Ordinal == fieldIndex).Name;
        }

        public int GetOrdinal(string fieldName)
        {
            return _request.ColumnHeaders.Single(x=>x.Name == fieldName).Ordinal;
        }

        public object GetValue(int fieldIndex)
        {
            return _enumerator.Current;
        }

        public bool Read()
        {
            return _enumerator.MoveNext();
        }
    }
}