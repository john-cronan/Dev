using System;
using System.Collections.Generic;

namespace JC.DataAccess.UnitTests
{
    internal sealed class TestingColumnDescriptorProvider : IColumnDescriptorProvider
    {
        private readonly IEnumerable<ColumnDescriptor> _Columns;

        public TestingColumnDescriptorProvider(IEnumerable<ColumnDescriptor> columns)
        {
            _Columns = columns;
        }

        public TestingColumnDescriptorProvider(ColumnDescriptor column)
        {
            _Columns = new ColumnDescriptor[] { column };
        }

        IEnumerable<ColumnDescriptor> IColumnDescriptorProvider.GetColumns(Type fromType)
        {
            return _Columns;
        }
    }
}
