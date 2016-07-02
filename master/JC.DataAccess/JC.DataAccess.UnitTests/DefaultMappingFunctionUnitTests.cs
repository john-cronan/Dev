using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JC.DataAccess.UnitTests
{
    [TestClass]
    public sealed class DefaultMappingFunctionUnitTests
    {
        [TestMethod]
        public void Assign_DBNull_to_int32_property()
        {
            var instance = Assign("IntProperty", DBNull.Value);
            Assert.IsTrue(instance.IntProperty == TestClass.DefaultInt32);
        }

        [TestMethod]
        public void Assign_null_to_int32_property()
        {
            var instance = Assign("IntProperty", null);
            Assert.IsTrue(instance.IntProperty == 0);
        }

        [TestMethod]
        public void Assign_int32_to_int32_property()
        {
            int value = int.MaxValue;
            var instance = Assign("IntProperty", value);
            Assert.IsTrue(instance.IntProperty == value);
        }

        [TestMethod]
        public void Assign_int16_to_int32_property()
        {
            short value = short.MaxValue;
            var instance = Assign("IntProperty", value);
            Assert.IsTrue(instance.IntProperty == value);
        }

        [TestMethod]
        public void Assign_byte_to_int32_property()
        {
            byte value = byte.MaxValue;
            var instance = Assign("IntProperty", value);
            Assert.IsTrue(instance.IntProperty == value);
        }

        [TestMethod]
        public void Assign_int16_to_int16_property()
        {
            short value = short.MaxValue;
            var instance = Assign("ShortProperty", value);
            Assert.IsTrue(instance.ShortProperty == value);
        }

        [TestMethod]
        public void Assign_int16_to_int64_property()
        {
            short value = short.MaxValue;
            var instance = Assign("LongProperty", value);
            Assert.IsTrue(instance.LongProperty == value);
        }

        [TestMethod]
        public void Assign_double_to_double_property()
        {
            double value = double.MaxValue;
            var instance = Assign("DoubleProperty", value);
            Assert.IsTrue(instance.DoubleProperty == value);
        }

        [TestMethod]
        public void Assign_single_to_double_property()
        {
            float value = float.MaxValue;
            var instance = Assign("DoubleProperty", value);
            Assert.IsTrue(instance.DoubleProperty == value);
        }

        [TestMethod]
        public void Assign_string_to_string_property()
        {
            string value = "Hello";
            var instance = Assign("StringProperty", value);
            Assert.IsTrue(instance.StringProperty == value);
        }

        [TestMethod]
        public void Assign_Guid_to_Guid_property()
        {
            Guid value = Guid.NewGuid();
            var instance = Assign("GuidProperty", value);
            Assert.IsTrue(instance.GuidProperty == value);
        }

        [TestMethod]
        public void Assign_DBNull_to_Guid_property()
        {
            var instance = Assign("GuidProperty", DBNull.Value);
            Assert.IsTrue(instance.GuidProperty == TestClass.DefaultGuid);
        }

        [TestMethod]
        public void Assign_bool_to_bool_property()
        {
            bool value = true;
            var instance = Assign("BoolProperty", value);
            Assert.IsTrue(instance.BoolProperty == value);
        }


        private TestClass Assign(string propertyName, object value)
        {
            string columnName = "database_column";
            var property = typeof(TestClass).GetProperty(propertyName);
            var resultset = new TestingDataReader();
            resultset.FieldValues.Add(columnName, value);
            var columnDescriptor = new ColumnDescriptor(property, columnName);
            var descriptorProvider = new TestingColumnDescriptorProvider(columnDescriptor);
            var instance = MappingFunction.Default<TestClass>(descriptorProvider, resultset);
            return instance;
        }


        private class TestClass
        {
            public const short DefaultInt16 = 42;
            public const int DefaultInt32 = 42;
            public const long DefaultInt64 = 42;
            public const double DefaultDouble = 42.5;
            public const string DefaultString = "_";
            public static readonly Guid DefaultGuid = Guid.NewGuid();
            public const bool DefaultBool = false;

            public short ShortProperty { get; set; } = DefaultInt16;
            public int IntProperty { get; set; } = DefaultInt32;
            public long LongProperty { get; set; } = DefaultInt64;
            public double DoubleProperty { get; set; } = DefaultDouble;
            public string StringProperty { get; set; } = DefaultString;
            public Guid GuidProperty { get; set; } = DefaultGuid;
            public bool BoolProperty { get; set; } = DefaultBool;

        }
    }
}
