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
    public class WindowStateMinimizedToVisibleConverterTests
    {
        [TestMethod()]
        public void Convert_ShouldReturnVisibilityHidden_WhenWindowStateNormal()
        {
            WindowStateMaximizedToVisibleConverter windowStateMaximizedToVisibleConverter = new WindowStateMaximizedToVisibleConverter();
            var result = windowStateMaximizedToVisibleConverter.Convert(WindowState.Normal, null, null, null);

            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod()]
        public void Convert_ShouldReturnVisibilityVisible_WhenWindowStateNotNormal()
        {
            WindowStateMaximizedToVisibleConverter windowStateMaximizedToVisibleConverter = new WindowStateMaximizedToVisibleConverter();
            var result = windowStateMaximizedToVisibleConverter.Convert(WindowState.Maximized, null, null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod()]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            WindowStateMinimizedToVisibleConverter windowStateMinimizedToVisibleConverter = new WindowStateMinimizedToVisibleConverter();

            Assert.ThrowsException<NotImplementedException>(() => windowStateMinimizedToVisibleConverter.ConvertBack(new object(), null, null, null));
        }
    }
}