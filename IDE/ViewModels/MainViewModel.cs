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
        private DebuggerModel _debuggerModel;
        private ConsoleModel _console;

        public EditorViewModel Editor { get; set; }
        public FileViewModel File { get; set; }
        public HelpViewModel Help { get; set; }
        public DebuggerViewModel Debugger { get; set; }

        public MainViewModel()
        {
            _document = new DocumentModel();
            _debuggerModel = new DebuggerModel();
            _console = new ConsoleModel();

            Editor = new EditorViewModel(_document);
            File = new FileViewModel(_document);
            Help = new HelpViewModel();
            Debugger = new DebuggerViewModel(_document, _console, _debuggerModel);
        }
    }
}
