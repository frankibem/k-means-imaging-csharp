using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void MaxIndexTest()
        {
            var list = new int[] { 5, 4, 23, 2, 0 };
            var expected = 2;

            var actual = EnumerableExtensions.MaxIndex(list);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MinIndexTest()
        {
            var list = new int[] { 5, 4, 23, 2, 0 };
            var expected = 4;

            var actual = EnumerableExtensions.MinIndex(list);
            Assert.AreEqual(expected, actual);
        }
    }
}