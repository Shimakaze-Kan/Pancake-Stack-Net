using IDE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IDE.ViewModels
{
    public class MainViewModel
    {
        private DocumentModel _document;
        private CancellationTokenSource _cancellationTokenSource;

        public EditorViewModel Editor { get; set; }
        public FileViewModel File { get; set; }
        public HelpViewModel Help { get; set; }        

        public MainViewModel()
        {
            _document = new DocumentModel();
            Editor = new EditorViewModel(_document, _cancellationTokenSource);
            File = new FileViewModel(_document, _cancellationTokenSource);
            Help = new HelpViewModel();
        }
    }
}
