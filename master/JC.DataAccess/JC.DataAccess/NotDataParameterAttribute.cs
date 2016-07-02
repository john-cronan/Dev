using System;

namespace JC.DataAccess
{
    /// <summary>
    /// Parameter descriptor providers ignore properties decorated with this
    /// attribute when returning descriptors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotDataParameterAttribute : Attribute
    {
    }
}
