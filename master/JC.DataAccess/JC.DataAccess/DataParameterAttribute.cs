using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace JC.DataAccess
{
    [ImmutableObject(true)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [DebuggerDisplay("Name = {Name}")]
    public sealed class DataParameterAttribute : Attribute
    {
        private readonly ParameterDirection _Direction;
        private readonly bool _IsNullable;
        private readonly string _Name;
        private readonly int _Ordinal;
        private readonly int _Size;
        private readonly DbType _Type;

        public DataParameterAttribute(string name, DbType type = DbType.String,
            ParameterDirection direction = ParameterDirection.Input, 
            bool isNullable = true, int size = -1, int ordinal = -1)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _Name = name;
            _Type = type;
            _Direction = direction;
            _IsNullable = isNullable;
            _Ordinal = ordinal;
            _Size = size;
        }

        public DbType Type
        {
            get { return _Type; }
        }

        public ParameterDirection Direction
        {
            get { return _Direction; }
        }

        public bool IsNullable
        {
            get { return _IsNullable; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public int Ordinal
        {
            get { return _Ordinal; }
        }

        public int Size
        {
            get { return _Size; }
        }
    }
}
