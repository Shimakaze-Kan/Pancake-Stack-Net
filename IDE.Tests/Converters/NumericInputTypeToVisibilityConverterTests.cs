using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IDE.Converters.Tests
{
    [TestClass()]
    public class NumericInputTypeToVisibilityConverterTests
    {
        [TestMethod()]
        public void Convert_ShouldReturnVisibilityVisible_WhenInputTypeIsNumeric()
        {
            NumericInputTypeToVisibilityConverter numericInputTypeToVisibilityConverter = new NumericInputTypeToVisibilityConverter();
            var result = numericInputTypeToVisibilityConverter.Convert(new WaitingForInputEventArgs() { Type = InputType.Numeric }, null, null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturnVisibilityHidden_WhenInputTypeIsNotNumeric()
        {
            NumericInputTypeToVisibilityConverter numericInputTypeToVisibilityConverter = new NumericInputTypeToVisibilityConverter();
            var result = numericInputTypeToVisibilityConverter.Convert(new WaitingForInputEventArgs() { Type = InputType.Alphanumeric }, null, null, null);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturnVisibilityHidden_WhenValueIsNull()
        {
            NumericInputTypeToVisibilityConverter numericInputTypeToVisibilityConverter = new NumericInputTypeToVisibilityConverter();
            var result = numericInputTypeToVisibilityConverter.Convert(null, null, null, null);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod()]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            NumericInputTypeToVisibilityConverter numericInputTypeToVisibilityConverter = new NumericInputTypeToVisibilityConverter();

            Assert.ThrowsException<NotImplementedException>(() => numericInputTypeToVisibilityConverter.ConvertBack(new object(), null, null, null));
        }
    }
}