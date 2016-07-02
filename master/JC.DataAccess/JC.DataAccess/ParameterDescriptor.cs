using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace JC.DataAccess
{
    [ImmutableObject(true)]
    public sealed class ParameterDescriptor
    {
        private readonly DbType _DbType;
        private readonly ParameterDirection _Direction;
        private readonly bool _IsNullable;
        private readonly PropertyInfo _Property;
        private readonly int _Ordinal;
        private readonly string _ParameterName;
        private readonly int _Size;
        private readonly Type _Type;

        public ParameterDescriptor(
            Type type = null, 
            PropertyInfo property = null,
            string parameterName = null, 
            DbType dbType = DbType.String, 
            ParameterDirection direction = ParameterDirection.Input,
            bool isNullable = true, 
            int size = -1, 
            int ordinal = -1)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentNullException(nameof(parameterName));

            _Type = type;
            _Property = property;
            _ParameterName = parameterName;
            _DbType = dbType;
            _Direction = direction;
            _Ordinal = ordinal;
            _Size = size;
            _IsNullable = isNullable;
        }

        public DbType DbType
        {
            get { return _DbType; }
        }

        public ParameterDirection Direction
        {
            get { return _Direction; }
        }

        public bool IsNullable
        {
            get { return _IsNullable; }
        }

        public PropertyInfo Property
        {
            get { return _Property; }
        }

        public string ParameterName
        {
            get { return _ParameterName; }
        }

        public int Ordinal
        {
            get { return _Ordinal; }
        }

        public int Size
        {
            get { return _Size; }
        }

        public Type Type
        {
            get { return _Type; }
        }
    }
}
