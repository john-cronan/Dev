using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace JC.DataAccess.UnitTests
{

    [TestClass]
    public sealed class DataParameterAttributeUnitTests
    {
        [TestMethod]
        public void All_properties_are_populated()
        {
            DataParameterAttribute testee = new DataParameterAttribute(
                "ParameterName", type: DbType.AnsiStringFixedLength,
                direction: ParameterDirection.InputOutput, isNullable: true,
                size: 1234, ordinal: -4);
            Assert.IsTrue(testee.Name == "ParameterName");
            Assert.IsTrue(testee.Type == DbType.AnsiStringFixedLength);
            Assert.IsTrue(testee.Direction == ParameterDirection.InputOutput);
            Assert.IsTrue(testee.IsNullable);
            Assert.IsTrue(testee.Size == 1234);
            Assert.IsTrue(testee.Ordinal == -4);
        }
    }
}
