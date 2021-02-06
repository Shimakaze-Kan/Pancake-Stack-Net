using IDE.Models;
using IDE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

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
        public DebugDocumentModel DebugDocument { get; set; }
        public DebuggerFlagsModel DebuggerFlags { get; set; }

        private EmbeddedInterpreter _embeddedInterpreter;
        private CancellationTokenSource _cancellationTokenSource;
        private WaitingForInputEventArgs _inputType;
        private Task _interpreterTask;
        private bool _isTaskRunning;
        private bool _isTaskWaitingForInput;
        private bool _isDebuggingMode;
        private int _previousInstruction;

        private EditorView _editorView;
        private EditorDebugView _editorDebugView;

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { OnPropertyChanged(ref _currentView, value); }
        }

        public bool IsDebuggingMode
        {
            get { return _isDebuggingMode; }
            set { OnPropertyChanged(ref _isDebuggingMode, value); }
        }

        public bool IsTaskWaitingForInput
        {
            get { return _isTaskWaitingForInput; }
            set { OnPropertyChanged(ref _isTaskWaitingForInput, value); }
        }

        public WaitingForInputEventArgs InputType
        {
            get { return _inputType; }
            set { OnPropertyChanged(ref _inputType, value); }
        }

        private Dictionary<int, int> _mapRealLineNumbersWithRaw;

        public DebuggerViewModel
            (DocumentModel document,
            DebugDocumentModel debugDocumentModel,
            ConsoleModel console,
            DebuggerModel debugger,
            object currentView,
            EditorView editorView,
            EditorDebugView editorDebugView,
            DebuggerFlagsModel debuggerFlags)
        {
            Debugger = debugger;
            Console = console;
            Document = document;
            DebugDocument = debugDocumentModel;
            DebuggerFlags = debuggerFlags;

            _currentView = currentView;
            _editorView = editorView;
            _editorDebugView = editorDebugView;

            _isTaskRunning = false;
            IsTaskWaitingForInput = false;
            IsDebuggingMode = false;
            _cancellationTokenSource = new CancellationTokenSource();

            RunInterpreterCommand = new RelayCommand(RunInterpreter, () => !_isTaskRunning && !string.IsNullOrEmpty(Document.Text) && !IsDebuggingMode);
            SendInputCommand = new RelayCommand(SendInput, () => IsTaskWaitingForInput);
            EndCurrentTaskCommand = new RelayCommand(EndCurrentInterpreterTask, () => _isTaskRunning || IsDebuggingMode);
            NextInstructionCommand = new RelayCommand(NextInstruction, () => !string.IsNullOrEmpty(Document.Text) && !_isTaskRunning);
        }

        private void RunInterpreter()
        {
            EndCurrentInterpreterTask();

            Console.ConsoleText = "";

            var validateCode = new ValidateSourceCode(Document.Text);
            _mapRealLineNumbersWithRaw = validateCode.MapRealLineNumbersWithRaw();

            if (validateCode.ValidateInstructions().Item1)
            {                
                _embeddedInterpreter = new EmbeddedInterpreter(validateCode.ValidSourceCode);
                AddHandlersToInterpreterThread();

                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = _cancellationTokenSource.Token;
                _interpreterTask = Task.Factory.StartNew(() =>
                    _embeddedInterpreter.StartExecuting(cancellationToken), cancellationToken);
                _isTaskRunning = true;
            }
            else
            {
                Console.ConsoleText = string.Format("Instruction on line {0} cannot be found{1}", 
                    _mapRealLineNumbersWithRaw[validateCode.ValidateInstructions().Item2]+1, Environment.NewLine);
            }
        }

        private void NextInstruction()
        {
            if (!IsDebuggingMode)
            {
                var validateCode = new ValidateSourceCode(Document.Text);
                _mapRealLineNumbersWithRaw = validateCode.MapRealLineNumbersWithRaw();

                if (validateCode.ValidateInstructions().Item1)
                {                    
                    _embeddedInterpreter = new EmbeddedInterpreter(validateCode.ValidSourceCode);
                    DebugDocument.Lines = new System.Collections.ObjectModel.ObservableCollection<TextLine>(Document.Text.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None).Select(x => new TextLine() { Text = x, BackgroundColor = Brushes.Transparent }));

                    var startLine = 0;
                    _mapRealLineNumbersWithRaw.TryGetValue(0, out startLine);
                    DebugDocument.LinePointer = startLine;
                    _previousInstruction = startLine;
                    CurrentView = _editorDebugView;

                    Console.ConsoleText = "";
                    AddHandlersToInterpreterThread();

                    IsDebuggingMode = true;
                    Debugger.Stack = null;
                    Debugger.Label = null;
                }
                else
                {
                    Console.ConsoleText = string.Format("Instruction on line {0} cannot be found{1}",
                    _mapRealLineNumbersWithRaw[validateCode.ValidateInstructions().Item2] + 1, Environment.NewLine);
                }
            }
            else if (!IsTaskWaitingForInput)
            {
                DebugDocument.Lines[_previousInstruction].BackgroundColor = Brushes.Transparent;
                DebugDocument.Lines[_previousInstruction].ForegroundColor = Brushes.White;

                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = _cancellationTokenSource.Token;
                _interpreterTask = Task.Factory.StartNew(() =>
                    _embeddedInterpreter.ExecuteNext(cancellationToken), cancellationToken).ContinueWith((_) => 
                    {
                        var line = 0; // if error
                        _mapRealLineNumbersWithRaw.TryGetValue(_embeddedInterpreter.ProgramIterator, out line);
                        DebugDocument.LinePointer = line; 
                        _previousInstruction = line; 
                    });
            }
        }

        private void SendInput()
        {
            if(DebuggerFlags.NoInputInConsoleFlag == false)
                Console.ConsoleText += ">" + Console.InputText + Environment.NewLine;

            _embeddedInterpreter.Input = Console.InputText + (DebuggerFlags.NoNewLineFlag ? "" : Environment.NewLine);
            IsTaskWaitingForInput = false;
            InputType = null;
            _embeddedInterpreter.WaitHandle.Set();
            Console.InputText = "";
        }

        private void EndCurrentInterpreterTask()
        {
            _cancellationTokenSource?.Cancel();
            _embeddedInterpreter?.WaitHandle.Set();

            if (IsDebuggingMode)
            {
                IsDebuggingMode = false;
                CurrentView = _editorView;
            }
            if (_isTaskRunning)
            {
                _isTaskRunning = false;
            }

            IsTaskWaitingForInput = false;
            InputType = null;
        }

        private void AddHandlersToInterpreterThread()
        {            
            _embeddedInterpreter.NewOutputEvent +=
            new EventHandler<OutputEventArgs>(delegate (Object o, OutputEventArgs a) // TODO change to dispatcher
                {
                    if (a.Type == OutputType.Character)
                        Console.ConsoleText += a.CharacterOutput;
                    else
                        Console.ConsoleText += a.LineOutput + (a.RuntimeError ? "" : Environment.NewLine);

                    if (a.RuntimeError)
                        Console.ConsoleText += _mapRealLineNumbersWithRaw[a.LineNumberErrorHandling] + 1 + Environment.NewLine;
                });
            _embeddedInterpreter.WaitingForInputEvent += new EventHandler<WaitingForInputEventArgs>((_, a) => { IsTaskWaitingForInput = true; InputType = a; });
            _embeddedInterpreter.PancakeStackChangedEvent += new EventHandler<Stack<int>>((o, a) => Debugger.Stack = a.ToList());
            _embeddedInterpreter.LabelDictionaryChangedEvent += new EventHandler<Dictionary<string, int>>((_, a) => Debugger.Label = a.Select(x => new KeyValuePair<string, int>(x.Key, x.Value + 2)).ToList());
            _embeddedInterpreter.EndOfExecutionEvent += new EventHandler((o, a) => EndCurrentInterpreterTask());
        }
    }
}
