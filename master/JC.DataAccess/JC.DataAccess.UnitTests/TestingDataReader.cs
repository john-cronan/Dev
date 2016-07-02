using System;
using System.Collections.Generic;
using System.Data;

namespace JC.DataAccess.UnitTests
{
    internal sealed class TestingDataReader : IDataReader
    {
        private readonly IDictionary<string, object> _FieldValues;

        public TestingDataReader()
        {
            _FieldValues = new Dictionary<string, object>();
        }

        public IDictionary<string,object> FieldValues
        {
            get { return _FieldValues; }
        }

        object IDataRecord.this[string name]
        {
            get
            {
                return _FieldValues[name];
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IDataReader.Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IDataRecord.FieldCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IDataReader.IsClosed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IDataReader.RecordsAffected
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void IDataReader.Close()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        bool IDataRecord.GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        byte IDataRecord.GetByte(int i)
        {
            throw new NotImplementedException();
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        char IDataRecord.GetChar(int i)
        {
            throw new NotImplementedException();
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        IDataReader IDataRecord.GetData(int i)
        {
            throw new NotImplementedException();
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        double IDataRecord.GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        Type IDataRecord.GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        float IDataRecord.GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        Guid IDataRecord.GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        short IDataRecord.GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        long IDataRecord.GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        string IDataRecord.GetName(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        DataTable IDataReader.GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        string IDataRecord.GetString(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.GetValue(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        bool IDataRecord.IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        bool IDataReader.NextResult()
        {
            throw new NotImplementedException();
        }

        bool IDataReader.Read()
        {
            throw new NotImplementedException();
        }
    }
}
