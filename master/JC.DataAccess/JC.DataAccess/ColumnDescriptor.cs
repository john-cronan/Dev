using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace JC.DataAccess
{
    [ImmutableObject(true)]
    [DebuggerDisplay("{ColumnName}")]
    public sealed class ColumnDescriptor
    {
        private readonly string _ColumnName;
        private readonly PropertyInfo _Property;

        public ColumnDescriptor(PropertyInfo property, string columnName)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            _ColumnName = columnName;
            _Property = property;
        }

        public string ColumnName
        {
            get { return _ColumnName; }
        }

        public PropertyInfo Property
        {
            get { return _Property; }
        }
    }
}
