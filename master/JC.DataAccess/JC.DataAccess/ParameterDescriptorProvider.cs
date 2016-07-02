using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace JC.DataAccess
{
    /// <summary>
    /// Provides the default implementation of 
    /// <see cref="IParameterDescriptorProvider"/>, caching the descriptors in
    /// memory.
    /// </summary>
    public sealed class ParameterDescriptorProvider : IParameterDescriptorProvider
    {
        private readonly ConcurrentDictionary<Type, IEnumerable<ParameterDescriptor>> 
            _Descriptors;


        public ParameterDescriptorProvider()
        {
            _Descriptors = new ConcurrentDictionary<Type, IEnumerable<ParameterDescriptor>>();
        }

        IEnumerable<ParameterDescriptor> IParameterDescriptorProvider.GetParameters(Type forType)
        {
            return _Descriptors.GetOrAdd(forType, ReflectType);
        }

        private IEnumerable<ParameterDescriptor> ReflectType(Type t)
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var properties = t.GetProperties(bindingFlags);
            var descriptors =
                from i in Enumerable.Range(0, properties.Length)
                let property = properties[i]
                let attribute = property.GetCustomAttribute<NotDataParameterAttribute>()
                where attribute == null
                select BuildDataParameterDescriptor(property, i);
            return descriptors.ToArray();
        }

        private ParameterDescriptor BuildDataParameterDescriptor(PropertyInfo property,
            int ordinal)
        {
            var attribute = property.GetCustomAttribute<DataParameterAttribute>(true);
            if (attribute == null)
            {
                return ReflectFromProperty(property, ordinal);
            }
            else
            {
                return ReflectFromAttribute(property, attribute, ordinal);
            }
        }

        private ParameterDescriptor ReflectFromAttribute(
            PropertyInfo property, DataParameterAttribute attribute,
            int ordinal)
        {
            return new ParameterDescriptor
            (
                type: property.PropertyType,
                property: property,
                parameterName: attribute.Name,
                dbType: attribute.Type,
                direction: attribute.Direction,
                isNullable: attribute.IsNullable,
                size: attribute.Size,
                ordinal: attribute.Ordinal
            );
        }

        private ParameterDescriptor ReflectFromProperty(PropertyInfo property,
            int ordinal)
        {
            string parameterName = property.Name;
            DbType parameterType;
            bool isNullable = true;
            InspectType(property.PropertyType, out parameterType);
            var direction = ParameterDirection.Input;
            int size = -1;
            return new ParameterDescriptor
            (
                type: property.PropertyType,
                property: property,
                parameterName: property.Name,
                dbType: parameterType,
                direction: direction,
                isNullable: isNullable,
                size: size,
                ordinal: ordinal
            );
        }

        private void InspectType(Type t, out DbType dbType)
        {
            if (t == typeof(byte[]))
            {
                dbType = DbType.Binary;
            }
            else if (t == typeof(byte))
            {
                dbType = DbType.Byte;
            }
            else if (t == typeof(byte?))
            {
                dbType = DbType.Byte;
            }
            else if (t == typeof(bool))
            {
                dbType = DbType.Boolean;
            }
            else if (t == typeof(bool?))
            {
                dbType = DbType.Boolean;
            }
            else if (t == typeof(DateTime))
            {
                dbType = DbType.DateTime2;
            }
            else if (t == typeof(DateTime?))
            {
                dbType = DbType.DateTime2;
            }
            else if (t == typeof(decimal))
            {
                dbType = DbType.Currency;
            }
            else if (t == typeof(decimal?))
            {
                dbType = DbType.Currency;
            }
            else if (t == typeof(double))
            {
                dbType = DbType.Double;
            }
            else if (t == typeof(double?))
            {
                dbType = DbType.Double;
            }
            else if (t == typeof(Guid))
            {
                dbType = DbType.Guid;
            }
            else if (t == typeof(Guid?))
            {
                dbType = DbType.Guid;
            }
            else if (t == typeof(short))
            {
                dbType = DbType.Int16;
            }
            else if (t == typeof(short?))
            {
                dbType = DbType.Int16;
            }
            else if (t == typeof(int))
            {
                dbType = DbType.Int32;
            }
            else if (t == typeof(int?))
            {
                dbType = DbType.Int32;
            }
            else if (t == typeof(long))
            {
                dbType = DbType.Int64;
            }
            else if (t == typeof(long?))
            {
                dbType = DbType.Int64;
            }
            else if (t == typeof(float))
            {
                dbType = DbType.Single;
            }
            else if (t == typeof(string))
            {
                dbType = DbType.String;
            }
            else if (t == typeof(XmlDocument))
            {
                dbType = DbType.Xml;
            }
            else if (t == typeof(XmlNode) || t == typeof(XmlElement))
            {
                dbType = DbType.Xml;
            }
            else if (t == typeof(XDocument))
            {
                dbType = DbType.Xml;
            }
            else if (t == typeof(XNode) || t == typeof(XElement))
            {
                dbType = DbType.Xml;
            }
            else if (t == typeof(XmlReader))
            {
                dbType = DbType.Xml;
            }
            else
            {
                dbType = DbType.Object;
            }
        }

    }
}
