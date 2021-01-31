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
    public class DebuggerViewModel : ObservableObject
    {
        public ICommand SendInputCommand { get; }
        public ICommand EndCurrentTaskCommand { get; }
        public ICommand NextInstructionCommand { get; }
        public ICommand RunInterpreterCommand { get; }
        public DebuggerModel Debugger { get; set; }
        public DocumentModel Document { get; set; }
        public ConsoleModel Console { get; set; }

        private EmbeddedInterpreter _embeddedInterpreter;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _interpreterTask;
        private bool _isTaskRunning;
        private bool _isTaskWaitingForInput;
        private bool _isDebbugingMode;

        //private object _currentView;
        private EditorView _editorView;
        private EditorDebugView _editorDebugView;

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { OnPropertyChanged(ref _currentView, value); }
        }


        public DebuggerViewModel(DocumentModel document, ConsoleModel console, DebuggerModel debugger, object currentView, EditorView editorView, EditorDebugView editorDebugView)
        {
            Debugger = debugger;
            Console = console;
            Document = document;

            _currentView = currentView;
            _editorView = editorView;
            _editorDebugView = editorDebugView;

            _isTaskRunning = false;
            _isTaskWaitingForInput = false;
            _isDebbugingMode = false;
            _cancellationTokenSource = new CancellationTokenSource();

            RunInterpreterCommand = new RelayCommand(RunInterpreter, () => !_isTaskRunning && !string.IsNullOrEmpty(Document.Text) && !_isDebbugingMode);
            SendInputCommand = new RelayCommand(SendInput, () => _isTaskWaitingForInput);
            EndCurrentTaskCommand = new RelayCommand(EndCurrentInterpreterTask, () => _isTaskRunning || _isDebbugingMode);
            NextInstructionCommand = new RelayCommand(NextInstruction, () => !string.IsNullOrEmpty(Document.Text) && !_isTaskRunning);
        }

        private void RunInterpreter()
        {
            EndCurrentInterpreterTask();

            Console.ConsoleText = "";
            AddHandlersToInterpreterThread();

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            _interpreterTask = Task.Factory.StartNew(() =>
                _embeddedInterpreter.StartExecuting(cancellationToken), cancellationToken);
            _isTaskRunning = true;
        }

        private void NextInstruction()
        {
            if (!_isDebbugingMode)
            {
                CurrentView = _editorDebugView;

                Console.ConsoleText = "";
                AddHandlersToInterpreterThread();

                _isDebbugingMode = true;
                Debugger.Stack = null;
                Debugger.Label = null;
            }
            else if (!_isTaskWaitingForInput)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = _cancellationTokenSource.Token;
                _interpreterTask = Task.Factory.StartNew(() =>
                    _embeddedInterpreter.ExecuteNext(cancellationToken), cancellationToken);
            }
        }

        private void SendInput()
        {
            Console.ConsoleText += ">" + Console.InputText + Environment.NewLine;
            _embeddedInterpreter.Input = Console.InputText + Environment.NewLine;
            _embeddedInterpreter.WaitHandle.Set();
            Console.InputText = "";
            _isTaskWaitingForInput = false;
        }

        private void EndCurrentInterpreterTask()
        {
            _cancellationTokenSource?.Cancel();
            _embeddedInterpreter?.WaitHandle.Set();

            if (_isDebbugingMode)
            {
                _isDebbugingMode = false;
                CurrentView = _editorView;
            }
            if (_isTaskRunning)
            {
                _isTaskRunning = false;
            }
        }

        private void AddHandlersToInterpreterThread()
        {
            _embeddedInterpreter = new EmbeddedInterpreter(Document.Text);
            _embeddedInterpreter.NewOutputEvent +=
            new EventHandler<OutputEventArgs>(delegate (Object o, OutputEventArgs a) // TODO change to dispatcher
                {
                    if (a.Type == OutputType.Character)
                        Console.ConsoleText += a.CharacterOutput;
                    else
                        Console.ConsoleText += a.LineOutput + Environment.NewLine;
                });
            _embeddedInterpreter.WaitingForInputEvent += new EventHandler((o, a) => _isTaskWaitingForInput = true);
            _embeddedInterpreter.PancakeStackChangedEvent += new EventHandler<Stack<int>>((o, a) => Debugger.Stack = a.ToList());
            _embeddedInterpreter.LabelDictionaryChangedEvent += new EventHandler<Dictionary<string, int>>((_, a) => Debugger.Label = a.Select(x => new KeyValuePair<string, int>(x.Key, x.Value + 2)).ToList());
            _embeddedInterpreter.EndOfExecutionEvent += new EventHandler((o, a) => EndCurrentInterpreterTask());
        }
    }
}
