using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.ViewModels.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        [TestMethod()]
        public void Thickness_ShouldExpandBorder_WhenMaximize()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.NormalCommand.Execute(null);

            var before = mainWindowViewModel.Thickness;

            mainWindowViewModel.MaximizeCommand.Execute(null);

            var after = mainWindowViewModel.Thickness;

            Assert.AreNotEqual(before, after);
        }

        [TestMethod()]
        public void Thickness_ShouldShrinkBorder_WhenNormal()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.MaximizeCommand.Execute(null);

            var before = mainWindowViewModel.Thickness;

            mainWindowViewModel.NormalCommand.Execute(null);

            var after = mainWindowViewModel.Thickness;

            Assert.AreNotEqual(before, after);
        }
    }
}