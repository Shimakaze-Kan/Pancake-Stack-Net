using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IDE.Converters.Tests
{
    [TestClass()]
    public class BottomBarColorConverterTests
    {
        [TestMethod()]
        public void Convert_ShouldReturnSolidBrushHexFF9912_WhenValueIsTrue()
        {
            BottomBarColorConverter bottomBarColorConverter = new BottomBarColorConverter();

            var result = bottomBarColorConverter.Convert(true, null, null, null);

            Assert.AreEqual(((SolidColorBrush)new BrushConverter().ConvertFrom("#FF9912")).ToString(), result.ToString());
        }

        [TestMethod()]
        public void Convert_ShouldReturnSolidBrushHex3A66A7_WhenValueIsFalse()
        {
            BottomBarColorConverter bottomBarColorConverter = new BottomBarColorConverter();

            var result = bottomBarColorConverter.Convert(false, null, null, null);

            Assert.AreEqual(((SolidColorBrush)new BrushConverter().ConvertFrom("#3A66A7")).ToString(), result.ToString());
        }

        [TestMethod()]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            BottomBarColorConverter bottomBarColorConverter = new BottomBarColorConverter();

            Assert.ThrowsException<NotImplementedException>(() => bottomBarColorConverter.ConvertBack(new object(), null, null, null));
        }
    }
}