using IDE.Models;
using IDE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDE.ViewModels
{
    /// <summary>
    /// Main viewmodel class, keeps all models up to date among all viewmodels
    /// </summary>
    public class MainViewModel
    {
        private DocumentModel _document;
        private DebuggerModel _debuggerModel;
        private ConsoleModel _console;
        private DebugDocumentModel _debugDocumentModel;
        private ReplaceModel _replaceModel;
        private CompilerFlagsModel _compilerFlags;
        private DebuggerFlagsModel _debuggerFlags;

        private EditorView _editorView;
        private EditorDebugView _editorDebugView;
        private object _currentView;

        public EditorViewModel Editor { get; set; }
        public FileViewModel File { get; set; }
        public HelpViewModel Help { get; set; }
        public DebuggerViewModel Debugger { get; set; }
        public CompilerViewModel Compiler { get; set; }
        public MainWindowViewModel MainWindow { get; set; }
        public ReplaceViewModel Replace { get; set; }

        public MainViewModel()
        {
            _document = new DocumentModel();
            _debuggerModel = new DebuggerModel();
            _console = new ConsoleModel();
            _debugDocumentModel = new DebugDocumentModel();
            _replaceModel = new ReplaceModel();
            _compilerFlags = new CompilerFlagsModel();
            _debuggerFlags = new DebuggerFlagsModel();

            _editorView = new EditorView();
            _editorDebugView = new EditorDebugView();
            _currentView = _editorView;

            Editor = new EditorViewModel(_document);
            File = new FileViewModel(_document);
            Help = new HelpViewModel();
            Debugger = new DebuggerViewModel(_document,_debugDocumentModel , _console, _debuggerModel, _currentView, _editorView, _editorDebugView, _debuggerFlags);
            Compiler = new CompilerViewModel(_document, _console, _compilerFlags);
            MainWindow = new MainWindowViewModel();
            Replace = new ReplaceViewModel(_replaceModel, _document);
        }
    }
}
