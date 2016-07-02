using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace JC.DataAccess.UnitTests
{
    [TestClass]
    public sealed class ColumnDescriptorProviderUnitTests
    {
        [TestMethod]
        public void Ignores_properties_with_NotDataColumnAttribute()
        {
            IColumnDescriptorProvider testee = new ColumnDescriptorProvider();
            var descriptors = testee.GetColumns(typeof(TestDataObject));
            Assert.IsFalse(descriptors.Any(x => x.Property.Name == "ColumnC"));
        }

        [TestMethod]
        public void Column_name_comes_from_property_name()
        {
            IColumnDescriptorProvider testee = new ColumnDescriptorProvider();
            var descriptors = testee.GetColumns(typeof(TestDataObject));
            int count = descriptors.Count(x => x.ColumnName == "ColumnA");
            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        public void Column_name_comes_from_attribute()
        {
            IColumnDescriptorProvider testee = new ColumnDescriptorProvider();
            var descriptors = testee.GetColumns(typeof(TestDataObject));
            int count = descriptors.Count(x => x.ColumnName == "column_b");
            Assert.IsTrue(count == 1);
        }

        private class TestDataObject
        {
            public int ColumnA { get; set; }

            [DataColumn("column_b")]
            public int ColumnB { get; set; }

            [NotDataColumn]
            public int ColumnC { get; set; }
        }
    }
}
