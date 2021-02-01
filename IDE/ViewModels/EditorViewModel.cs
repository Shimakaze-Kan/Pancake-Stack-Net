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
        public FormatModel Format { get; set; }
        public DocumentModel Document { get; set; }
        public ListViewItem HelpBoxInstruction { get { return null; } set { Document.Text += value.Content + Environment.NewLine; } }

        public EditorViewModel(DocumentModel document)
        {
            Document = document;         
            Format = new FormatModel() { Family = new FontFamily("Consolas"), Size = 18 };
            FormatCommand = new RelayCommand(OpenFormatDialog);
            WrapCommand = new RelayCommand(ToggleWrap);            
        }

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
    }
}
