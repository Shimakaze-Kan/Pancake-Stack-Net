using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace IDE.Converters.Tests
{
    [TestClass()]
    public class AlphanumericInputTypeToVisibilityConverterTests
    {
        [TestMethod()]
        public void Convert_ShouldReturnVisibilityVisible_WhenInputTypeIsAlphanumeric()
        {
            AlphanumericInputTypeToVisibilityConverter alphanumericInputTypeToVisibilityConverter = new AlphanumericInputTypeToVisibilityConverter();
            var result = alphanumericInputTypeToVisibilityConverter.Convert(new WaitingForInputEventArgs() { Type = InputType.Alphanumeric } , null, null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturnVisibilityHidden_WhenInputTypeIsNotAlphanumeric()
        {
            AlphanumericInputTypeToVisibilityConverter alphanumericInputTypeToVisibilityConverter = new AlphanumericInputTypeToVisibilityConverter();
            var result = alphanumericInputTypeToVisibilityConverter.Convert(new WaitingForInputEventArgs() { Type = InputType.Numeric }, null, null, null);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturnVisibilityHidden_WhenValueIsNull()
        {
            AlphanumericInputTypeToVisibilityConverter alphanumericInputTypeToVisibilityConverter = new AlphanumericInputTypeToVisibilityConverter();
            var result = alphanumericInputTypeToVisibilityConverter.Convert(null, null, null, null);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod()]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            AlphanumericInputTypeToVisibilityConverter alphanumericInputTypeToVisibilityConverter = new AlphanumericInputTypeToVisibilityConverter();

            Assert.ThrowsException<NotImplementedException>(() => alphanumericInputTypeToVisibilityConverter.ConvertBack(new object(), null, null, null));
        }
    }
}