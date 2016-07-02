using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace JC.DataAccess.UnitTests
{
    [TestClass]
    public sealed class DataParameterDescriptorUnitTests
    {
        [TestMethod]
        public void All_properties_are_populated()
        {
            var property = typeof(TestClass).GetProperty("Property");
            ParameterDescriptor testee = new ParameterDescriptor(
                typeof(TestClass), property, "Parameter", 
                DbType.AnsiStringFixedLength, ParameterDirection.InputOutput, 
                true, 1234, -4);
            Assert.IsTrue(testee.Type == typeof(TestClass));
            Assert.IsTrue(testee.ParameterName == "Parameter");
            Assert.IsTrue(testee.Property == property);
            Assert.IsTrue(testee.DbType == DbType.AnsiStringFixedLength);
            Assert.IsTrue(testee.Direction == ParameterDirection.InputOutput);
            Assert.IsTrue(testee.IsNullable);
            Assert.IsTrue(testee.Size == 1234);
            Assert.IsTrue(testee.Ordinal == -4);
        }

        private class TestClass
        {
            public int Property { get; set; }
        }
    }
}
