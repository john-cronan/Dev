using System;
using System.Collections.Generic;

namespace JC.DataAccess
{
    /// <summary>
    /// Defines the members implemented by a component that returns a sequence of
    /// <see cref="ParameterDescriptor"/> objects for a class that represents the
    /// parameters passed to a database command.
    /// </summary>
    public interface IParameterDescriptorProvider
    {
        /// <summary>
        /// Returns a sequence of <see cref="ParameterDescriptor"/> objects by
        /// reading a class' properties and those properties' attributes. The 
        /// inspected type must be a named or anonymous class.
        /// </summary>
        IEnumerable<ParameterDescriptor> GetParameters(Type forType);
    }
}