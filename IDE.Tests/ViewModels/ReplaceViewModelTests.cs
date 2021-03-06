﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Models;

namespace IDE.ViewModels.Tests
{
    [TestClass()]
    public class ReplaceViewModelTests
    {
        [TestMethod()]
        public void ChangeVisiblilityOfReplaceWindow_ShouldChangeVisibilityOfReplaceWindow()
        {
            ReplaceViewModel replaceViewModel = new ReplaceViewModel(new ReplaceModel(), new DocumentModel());
            var before = replaceViewModel.Replace.IsVisible;

            replaceViewModel.ChangeVisiblilityOfReplaceWindowCommand.Execute(null);

            var after = replaceViewModel.Replace.IsVisible;

            Assert.AreNotEqual(before, after);
        }

        [TestMethod()]
        public void ReplaceText_ShouldReplaceText_WhenNoRegex()
        {
            var text = "test nontest word";
            var expected = "nan nonnan word";

            ReplaceModel replaceModel = new ReplaceModel();
            DocumentModel documentModel = new DocumentModel();

            replaceModel.IsRegex = false;
            replaceModel.TextToReplace = "test";
            replaceModel.TextAfterReplace = "nan";

            documentModel.Text = text;

            ReplaceViewModel replaceViewModel = new ReplaceViewModel(replaceModel, documentModel);

            replaceViewModel.ReplaceTextCommand.Execute(null);

            Assert.AreEqual(expected, documentModel.Text);
        }

        [TestMethod()]
        public void ReplaceText_ShouldReplaceText_WhenRegex()
        {
            var text = "test nontest word";
            var expected = "testnannontestnanword";

            ReplaceModel replaceModel = new ReplaceModel();
            DocumentModel documentModel = new DocumentModel();

            replaceModel.IsRegex = true;
            replaceModel.TextToReplace = @"\s+";
            replaceModel.TextAfterReplace = "nan";

            documentModel.Text = text;

            ReplaceViewModel replaceViewModel = new ReplaceViewModel(replaceModel, documentModel);

            replaceViewModel.ReplaceTextCommand.Execute(null);

            Assert.AreEqual(expected, documentModel.Text);
        }

        [TestMethod()]
        public void ReplaceText_ShouldBeAbleToExecute_WhenDocumentTextAndTextToReplaceAndReplacementAreNotNullOrEmpty()
        {
            DocumentModel documentModel = new DocumentModel();

            documentModel.Text = "test";

            ReplaceModel replaceModel = new ReplaceModel();

            replaceModel.TextToReplace = "test";
            replaceModel.TextAfterReplace = "test";
            
            ReplaceViewModel replaceViewModel = new ReplaceViewModel(replaceModel, documentModel);

            Assert.IsTrue(replaceViewModel.ReplaceTextCommand.CanExecute(null));
        }

        [TestMethod()]
        public void ReplaceText_ShouldNotBeAbleToExecute_WhenDocumentTextAndTextToReplaceAndReplacementAreNullOrEmpty()
        {
            DocumentModel documentModel = new DocumentModel();

            documentModel.Text = string.Empty;

            ReplaceModel replaceModel = new ReplaceModel();

            replaceModel.TextToReplace = string.Empty;
            replaceModel.TextAfterReplace = string.Empty;
            
            ReplaceViewModel replaceViewModel = new ReplaceViewModel(replaceModel, documentModel);

            Assert.IsFalse(replaceViewModel.ReplaceTextCommand.CanExecute(null));
        }
    }
}