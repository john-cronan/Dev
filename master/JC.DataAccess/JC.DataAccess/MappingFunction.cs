using System;
using System.Data;

namespace JC.DataAccess
{
    public static class MappingFunction
    {
        public static T Default<T>(IColumnDescriptorProvider columnDescriptors, 
            IDataReader resultset)
        {
            object instance = Activator.CreateInstance(typeof(T));
            foreach (var column in columnDescriptors.GetColumns(typeof(T)))
            {
                var value = resultset[column.ColumnName];
                if (value != DBNull.Value)
                {
                    //
                    //  This simple assignment doesn't allow narrowing conversions, even
                    //  when the values are OK. For example, if you have an int column
                    //  holding the value 42, this proceure won't assign it to a property
                    //  of type short, even though it fits. This could be considered a 
                    //  shortcoming or a feature depending on your POV. That assignment 
                    //  is inherently unsafe, even if it works in some particular instance.
                    column.Property.SetValue(instance, resultset[column.ColumnName]);
                }
            }
            return (T)instance;
        }
    }
}
