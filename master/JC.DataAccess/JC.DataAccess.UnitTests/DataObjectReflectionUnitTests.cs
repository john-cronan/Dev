using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace JC.DataAccess.UnitTests
{
    [TestClass]
    public sealed class DataObjectReflectionUnitTests
    {
        /// <summary>
        /// All of the reflection in the tested library operates on properties, not
        /// on fields. This test verifies that the attributes on anonymous types are
        /// acutally properties. If that's not the case-- or if it changes in the
        /// future-- that library is broken.
        /// </summary>
        [TestMethod]
        public void Anonymous_type_has_properties_not_fields()
        {
            var obj = new
            {
                FirstName = "John",
                LastName = "Smith"
            };
            Assert.IsTrue(obj.GetType().GetProperties().Length == 2);
            Assert.IsTrue(obj.GetType().GetProperty("FirstName") != null);
            Assert.IsTrue(obj.GetType().GetProperty("LastName") != null);
        }
    }
}
