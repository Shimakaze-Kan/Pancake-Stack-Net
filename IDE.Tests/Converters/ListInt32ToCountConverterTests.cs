using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Converters.Tests
{
    [TestClass()]
    public class ListInt32ToCountConverterTests
    {
        [TestMethod()]
        public void Convert_ShouldReturnNumberOfElements_WhenListIsNotEmpty()
        {
            ListInt32ToCountConverter listInt32ToCountConverter = new ListInt32ToCountConverter();
            var result = listInt32ToCountConverter.Convert(new List<int>() { 1, 2, 3 }, null, null, null);

            Assert.AreEqual(3, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturn0_WhenNull()
        {
            ListInt32ToCountConverter listInt32ToCountConverter = new ListInt32ToCountConverter();
            var result = listInt32ToCountConverter.Convert(null, null, null, null);

            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            AlphanumericInputTypeToVisibilityConverter alphanumericInputTypeToVisibilityConverter = new AlphanumericInputTypeToVisibilityConverter();

            Assert.ThrowsException<NotImplementedException>(() => alphanumericInputTypeToVisibilityConverter.ConvertBack(new object(), null, null, null));
        }
    }
}