using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDE.Models;
using System.Windows.Controls;

namespace IDE.ViewModels.Tests
{
    [TestClass()]
    public class EditorViewModelTests
    {
        [TestMethod()]
        public void ZoomInEditor_ShouldIncreaseFontSize_WhenFontSizeIsLessThan30()
        {
            EditorViewModel editorViewModel = new EditorViewModel(new DocumentModel());

            for (int i = 0; i < 100; i++)
            {
                editorViewModel.ZoomInEditorCommand.Execute(null);
            }

            Assert.AreEqual(30, editorViewModel.Format.Size);
        }

        [TestMethod()]
        public void ZoomOutEditor_ShouldReduceFontSize_WhenFontSizeIsGreaterThan5()
        {
            EditorViewModel editorViewModel = new EditorViewModel(new DocumentModel());

            for (int i = 100; i >= 0; i--)
            {
                editorViewModel.ZoomOutEditorCommand.Execute(null);
            }

            Assert.AreEqual(5, editorViewModel.Format.Size);
        }

        [TestMethod()]
        public void ToggleWrap_ShouldChangeWrapBehaviourToOposite()
        {
            EditorViewModel editorViewModel = new EditorViewModel(new DocumentModel());

            var before = editorViewModel.Format.Wrap;

            editorViewModel.WrapCommand.Execute(null);
            editorViewModel.WrapCommand.Execute(null);
            editorViewModel.WrapCommand.Execute(null);

            var after = editorViewModel.Format.Wrap;

            Assert.AreNotEqual(before, after);
        }

        [TestMethod()]
        public void HelpBoxInstruction_ShouldAddInstructionAtTheEndOfDocumentText_WhenListViewItemClicked()
        {
            var text1 = "Test1";
            var text2 = "Test2";
            var expected = text1 + Environment.NewLine + text2 + Environment.NewLine;
            EditorViewModel editorViewModel = new EditorViewModel(new DocumentModel());
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = text1;

            editorViewModel.HelpBoxInstruction = listViewItem;

            listViewItem.Content = text2;

            editorViewModel.HelpBoxInstruction = listViewItem;

            Assert.AreEqual(expected, editorViewModel.Document.Text);
        }

        [TestMethod()]
        public void HelpBoxInstruction_ShouldReturnNull_WhenGet()
        {
            EditorViewModel editorViewModel = new EditorViewModel(new DocumentModel());

            var value = editorViewModel.HelpBoxInstruction;

            Assert.IsNull(value);
        }
    }
}