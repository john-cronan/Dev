using System;

namespace JC.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DataColumnAttribute : Attribute
    {
        private readonly string _ColumnName;

        public DataColumnAttribute(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            _ColumnName = columnName;
        }

        public string ColumnName
        {
            get { return _ColumnName; }
        }
    }
}
