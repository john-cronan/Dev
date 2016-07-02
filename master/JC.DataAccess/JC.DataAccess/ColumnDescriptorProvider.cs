using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JC.DataAccess
{
    /// <summary>
    /// Provides the default implementation of <see cref="IColumnDescriptorProvider"/>,
    /// caching the descriptors in memory.
    /// </summary>
    public sealed class ColumnDescriptorProvider : IColumnDescriptorProvider
    {
        private readonly ConcurrentDictionary<Type, IEnumerable<ColumnDescriptor>>
            _Descriptors;

        public ColumnDescriptorProvider()
        {
            _Descriptors = new ConcurrentDictionary<Type, IEnumerable<ColumnDescriptor>>();
        }

        IEnumerable<ColumnDescriptor> IColumnDescriptorProvider.GetColumns(Type fromType)
        {
            return _Descriptors.GetOrAdd(fromType, ReflectType);
        }

        private IEnumerable<ColumnDescriptor> ReflectType(Type t)
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var q =
                from property in t.GetProperties(bindingFlags)
                let attribute = property.GetCustomAttribute<NotDataColumnAttribute>()
                where attribute == null
                select BuildDescriptor(property);
            return q.ToArray();
        }

        private ColumnDescriptor BuildDescriptor(PropertyInfo fromProperty)
        {
            string propertyName = fromProperty.Name;
            var attribute = fromProperty.GetCustomAttribute<DataColumnAttribute>();
            string columnName = attribute == null ? propertyName : attribute.ColumnName;
            return new ColumnDescriptor(fromProperty, columnName);
        }
    }
}
