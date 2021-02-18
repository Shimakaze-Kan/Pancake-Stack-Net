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
    class BoolToVisiblilityConverterTests
    {
        [TestMethod()]
        public void Convert_ShouldReturnVisibilityVisable_WhenValueIsTrue()
        {
            BoolToVisiblilityConverter boolToVisiblilityConverter = new BoolToVisiblilityConverter();

            var result = boolToVisiblilityConverter.Convert(true, null, null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturnVisibilityHidden_WhenValueIsFalse()
        {
            BoolToVisiblilityConverter boolToVisiblilityConverter = new BoolToVisiblilityConverter();

            var result = boolToVisiblilityConverter.Convert(false, null, null, null);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod()]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            BoolToVisiblilityConverter boolToVisiblilityConverter = new BoolToVisiblilityConverter();

            Assert.ThrowsException<NotImplementedException>(() => boolToVisiblilityConverter.ConvertBack(new object(), null, null, null));
        }
    }
}