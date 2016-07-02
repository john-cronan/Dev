using System;

namespace JC.DataAccess
{
    /// <summary>
    /// Column descriptor providers will ignore properties decorated with this
    /// attribute when returning descriptors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotDataColumnAttribute : Attribute
    {
    }
}
