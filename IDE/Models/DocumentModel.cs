using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Models
{
    public class DocumentModel : ObservableObject
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set 
            { 
                OnPropertyChanged(ref _text, value);
                LineNumber = new ObservableCollection<int>(Enumerable.Range(1, value.Split('\n').Length));
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { OnPropertyChanged(ref _filePath, value); }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { OnPropertyChanged(ref _fileName, value); }
        }

        private ObservableCollection<int> _lineNumber;

        public ObservableCollection<int> LineNumber
        {
            get { return _lineNumber; }
            set { OnPropertyChanged(ref _lineNumber, value); }
        }

    }
}
