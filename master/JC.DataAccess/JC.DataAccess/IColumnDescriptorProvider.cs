using System;
using System.Collections.Generic;

namespace JC.DataAccess
{
    /// <summary>
    /// Defines the members implemented by a component that returns a sequence
    /// of <see cref="ColumnDescriptor"/> objects for a class that represents a 
    /// row of a resultset.
    /// </summary>
    public interface IColumnDescriptorProvider
    {
        /// <summary>
        /// Returns a sequence of <see cref="ColumnDescriptor"/> objects by 
        /// inspecting the specified type's properties and those properties'
        /// attributes.
        /// </summary>
        IEnumerable<ColumnDescriptor> GetColumns(Type fromType);
    }
}
