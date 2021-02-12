using IDE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IDE.ViewModels
{
    public class EditorViewModel
    {
        public ICommand FormatCommand { get; }
        public ICommand WrapCommand { get; }
        public ICommand ZoomInEditorCommand { get; }
        public ICommand ZoomOutEditorCommand { get; }
        public FormatModel Format { get; set; }
        public DocumentModel Document { get; set; }
        public ListViewItem HelpBoxInstruction { get { return null; } set { Document.Text += value.Content + Environment.NewLine; } }

        public EditorViewModel(DocumentModel document)
        {
            Document = document;         
            Format = new FormatModel() { Family = new FontFamily("Consolas"), Size = 12 };
            FormatCommand = new RelayCommand(OpenFormatDialog);
            WrapCommand = new RelayCommand(ToggleWrap);
            ZoomInEditorCommand = new RelayCommand(ZoomInEditor);
            ZoomOutEditorCommand = new RelayCommand(ZoomOutEditor);
        }

        /// <summary>
        /// Changes text wrap behavior
        /// </summary>
        private void ToggleWrap()
        {
            if(Format.Wrap == System.Windows.TextWrapping.Wrap)
            {
                Format.Wrap = System.Windows.TextWrapping.NoWrap;
            }
            else
            {
                Format.Wrap = System.Windows.TextWrapping.Wrap;
            }
        }

        private void OpenFormatDialog()
        {
            var formatDialog = new FormatDialog
            {
                DataContext = Format
            };
            formatDialog.ShowDialog();
        }

        /// <summary>
        /// Increases font size
        /// </summary>
        private void ZoomInEditor()
        {
            if (Format.Size < 30)
                Format.Size++;
        }

        /// <summary>
        /// Reduces font size
        /// </summary>
        private void ZoomOutEditor()
        {
            if (Format.Size > 5)
                Format.Size--;
        }
    }
}
