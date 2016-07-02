using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JC.DataAccess.UnitTests
{
    [TestClass]
    public sealed class ParameterDescriptorProviderUnitTests
    {
        [TestMethod]
        public void Parameter_names_come_from_property_names()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(typeof(TestTypeWithoutAttributes));
            Assert.IsTrue(descriptors.All(x => x.ParameterName == x.Property.Name));
        }

        [TestMethod]
        public void Parameter_names_come_from_attributes()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            Func<ParameterDescriptor, bool> parameterIsOK = descriptor =>
            {
                bool returnValue = descriptor.Property.Name == string.Concat(descriptor.ParameterName, "Property");
                return returnValue;
            };
            Assert.IsTrue(descriptors.All(parameterIsOK));
        }

        [TestMethod]
        public void Parameter_direction_defaults_to_Input()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "DateTimeProperty");
            Assert.IsTrue(member.Direction == ParameterDirection.Input);

            descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithoutAttributes));
            member = descriptors.First(x => x.Property.Name == "DateTimeProperty");
            Assert.IsTrue(member.Direction == ParameterDirection.Input);
        }

        [TestMethod]
        public void IsNullable_defaults_to_true_for_reference_types()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "StringProperty");
            Assert.IsTrue(member.IsNullable);

            descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithoutAttributes));
            member = descriptors.First(x => x.Property.Name == "StringProperty");
            Assert.IsTrue(member.IsNullable);
        }

        [TestMethod]
        public void IsNullable_defaults_to_true_for_nullable_value_types()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "NullableShortProperty");
            Assert.IsTrue(member.IsNullable);

            descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithoutAttributes));
            member = descriptors.First(x => x.Property.Name == "NullableShortProperty");
            Assert.IsTrue(member.IsNullable);
        }

        [TestMethod]
        public void IsNullable_defaults_to_true_for_value_types()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "ShortProperty");
            Assert.IsTrue(member.IsNullable);

            descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithoutAttributes));
            member = descriptors.First(x => x.Property.Name == "ShortProperty");
            Assert.IsTrue(member.IsNullable);
        }

        [TestMethod]
        public void IsNullable_comes_from_attributes()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "StringProperty");
            Assert.IsTrue(member.IsNullable);

            member = descriptors.First(x => x.Property.Name == "IntProperty");
            Assert.IsTrue(!member.IsNullable);
        }

        [TestMethod]
        public void Parameter_direction_explicitly_set()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "ByteArrayProperty");
            Assert.IsTrue(member.Direction == ParameterDirection.Input);
            member = descriptors.First(x => x.Property.Name == "ByteProperty");
            Assert.IsTrue(member.Direction == ParameterDirection.InputOutput);
            member = descriptors.First(x => x.Property.Name == "NullableByteProperty");
            Assert.IsTrue(member.Direction == ParameterDirection.Output);
            member = descriptors.First(x => x.Property.Name == "BoolProperty");
            Assert.IsTrue(member.Direction == ParameterDirection.ReturnValue);
        }

        [TestMethod]
        public void Ordinal_comes_from_attributes()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "BoolProperty");
            Assert.IsTrue(member.Ordinal == 4);
            member = descriptors.First(x => x.Property.Name == "IntProperty");
            Assert.IsTrue(member.Ordinal == 16);
        }

        [TestMethod]
        public void Size_defaults_to_negative_one()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "IntProperty");
            Assert.IsTrue(member.Size == -1);

            descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithoutAttributes));
            member = descriptors.First(x => x.Property.Name == "IntProperty");
            Assert.IsTrue(member.Size == -1);
        }

        [TestMethod]
        public void Size_comes_from_attribute()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "StringProperty");
            Assert.IsTrue(member.Size == 47);
        }

        [TestMethod]
        public void Type_comes_from_attribute()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            var member = descriptors.First(x => x.Property.Name == "DecimalProperty");
            Assert.IsTrue(member.DbType == DbType.Decimal);
        }

        [TestMethod]
        public void Ignores_property_with_NotDataParameterAttribute()
        {
            IParameterDescriptorProvider descriptorProvider =
                new ParameterDescriptorProvider();
            var descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithAttributes));
            Assert.IsFalse(descriptors.Any(x => x.Property.Name == "ShouldIgnoreThisOne"));

            descriptors = descriptorProvider.GetParameters(
                typeof(TestTypeWithoutAttributes));
            Assert.IsFalse(descriptors.Any(x => x.Property.Name == "ShouldIgnoreThisOne"));
        }

        private class TestTypeWithoutAttributes
        {
            public byte[] ByteArrayProperty { get; set; }
            public byte ByteProperty { get; set; }
            public byte? NullableByteProperty { get; set; }
            public bool BoolProperty { get; set; }
            public bool? NullableBoolProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public DateTime? NullableDateTimeProperty { get; set; }
            public decimal DecimalProperty { get; set; }
            public decimal? NullableDecimalProperty { get; set; }
            public double DoubleProperty { get; set; }
            public double? NullableDoubleProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public Guid? NullableGuidProperty { get; set; }
            public short ShortProperty { get; set; }
            public short? NullableShortProperty { get; set; }
            public int IntProperty { get; set; }
            public int? NullableIntProperty { get; set; }
            public long LongProperty { get; set; }
            public long? NullableLongProperty { get; set; }
            public float FloatProperty { get; set; }
            public float? NullableFloatProperty { get; set; }
            public string StringProperty { get; set; }
            public XmlDocument XmlDocumentProperty { get; set; }
            public XmlNode XmlNodeProperty { get; set; }
            public XDocument XDocumentProperty { get; set; }
            public XElement XElementProperty { get; set; }
            public XmlReader XmlReaerProperty { get; set; }

            [NotDataParameter]
            public string ShouldIgnoreThisOne { get; set; }
        }


        private class TestTypeWithAttributes
        {
            [DataParameter("ByteArray", direction: ParameterDirection.Input,
                ordinal: 1)]
            public byte[] ByteArrayProperty { get; set; }

            [DataParameter("Byte", direction: ParameterDirection.InputOutput,
                ordinal: 2)]
            public byte ByteProperty { get; set; }

            [DataParameter("NullableByte", direction: ParameterDirection.Output,
                ordinal: 3)]
            public byte? NullableByteProperty { get; set; }

            [DataParameter("Bool", direction: ParameterDirection.ReturnValue,
                ordinal: 4)]
            public bool BoolProperty { get; set; }

            [DataParameter("NullableBool", ordinal: 5)]
            public bool? NullableBoolProperty { get; set; }

            [DataParameter("DateTime", ordinal: 6)]
            public DateTime DateTimeProperty { get; set; }

            [DataParameter("NullableDateTime", ordinal: 7)]
            public DateTime? NullableDateTimeProperty { get; set; }

            [DataParameter("Decimal", type: DbType.Decimal, ordinal: 8)]
            public decimal DecimalProperty { get; set; }

            [DataParameter("NullableDecimal", ordinal: 9)]
            public decimal? NullableDecimalProperty { get; set; }

            [DataParameter("Double", ordinal: 10)]
            public double DoubleProperty { get; set; }

            [DataParameter("NullableDouble", ordinal: 11)]
            public double? NullableDoubleProperty { get; set; }

            [DataParameter("Guid", ordinal: 12)]
            public Guid GuidProperty { get; set; }

            [DataParameter("NullableGuid", ordinal: 13)]
            public Guid? NullableGuidProperty { get; set; }

            [DataParameter("Short", ordinal: 14)]
            public short ShortProperty { get; set; }

            [DataParameter("NullableShort", ordinal: 15)]
            public short? NullableShortProperty { get; set; }

            [DataParameter("Int", isNullable: false, ordinal: 16)]
            public int IntProperty { get; set; }

            [DataParameter("NullableInt", isNullable: false, ordinal: 17)]
            public int? NullableIntProperty { get; set; }

            [DataParameter("Long", ordinal: 18)]
            public long LongProperty { get; set; }

            [DataParameter("NullableLong", ordinal: 19)]
            public long? NullableLongProperty { get; set; }

            [DataParameter("Float", ordinal: 20)]
            public float FloatProperty { get; set; }

            [DataParameter("NullableFloat", ordinal: 21)]
            public float? NullableFloatProperty { get; set; }

            [DataParameter("String", isNullable: true, ordinal: 22, size: 47)]
            public string StringProperty { get; set; }

            [DataParameter("XmlDocument", ordinal: 23)]
            public XmlDocument XmlDocumentProperty { get; set; }

            [DataParameter("XmlNode", ordinal: 24)]
            public XmlNode XmlNodeProperty { get; set; }

            [DataParameter("XDocument", ordinal: 25)]
            public XDocument XDocumentProperty { get; set; }

            [DataParameter("XElement", ordinal: 26)]
            public XElement XElementProperty { get; set; }

            [DataParameter("XmlReader", ordinal: 27)]
            public XmlReader XmlReaderProperty { get; set; }

            [NotDataParameter]
            public string ShouldIgnoreThisOne { get; set; }
        }

    }
}
