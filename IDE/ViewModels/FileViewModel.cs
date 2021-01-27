using IDE.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public class FileViewModel
    {
        public DocumentModel Document { get; private set; }

        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }

        public FileViewModel(DocumentModel document)
        {
            Document = document;
            NewCommand = new RelayCommand(NewFile);
            OpenCommand = new RelayCommand(OpenFile);
            SaveCommand = new RelayCommand(SaveFile);
            SaveAsCommand = new RelayCommand(SaveFileAs);
        }

        public void NewFile()
        {
            Document.FileName = "";
            Document.FilePath = "";
            Document.Text = "";
        }

        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pancake Stack Source Code (*.psc, *.txt)|*.psc;*.txt";

            if(openFileDialog.ShowDialog() == true)
            {
                DockFile(openFileDialog);
                Document.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        public void SaveFile()
        {
            File.WriteAllText(Document.FilePath, Document.Text);
        }

        public void SaveFileAs()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pancake Stack Source Code (*.psc, *.txt)|*.psc;*.txt";
            
            if(saveFileDialog.ShowDialog() == true)
            {
                DockFile(saveFileDialog);
                SaveFile();
            }
        }

        private void DockFile<T>(T dialog) where T : FileDialog
        {
            Document.FileName = dialog.SafeFileName;
            Document.FilePath = dialog.FileName;
        }
    }
}
