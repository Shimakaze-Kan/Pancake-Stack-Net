using IDE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IDE.ViewModels
{
    public class EditorViewModel
    {
        public ICommand FormatCommand { get; }
        public ICommand WrapCommand { get; }
        public ICommand RunInterpreterCommand { get; }
        public ICommand SendInputCommand { get; }
        public ICommand EndCurrentTaskCommand { get; }
        public FormatModel Format { get; set; }
        public DocumentModel Document { get; set; }
        public ConsoleModel Console { get; set; }

        private EmbeddedInterpreter _embeddedInterpreter;
        private CancellationTokenSource _cancellationTokenSource;

        public EditorViewModel(DocumentModel document, CancellationTokenSource cancellationTokenSource)
        {
            Console = new ConsoleModel();
            Document = document;
            _cancellationTokenSource = cancellationTokenSource;
            Format = new FormatModel() { Family = new FontFamily("Consolas"), Size = 18 };
            FormatCommand = new RelayCommand(OpenFormatDialog);
            WrapCommand = new RelayCommand(ToggleWrap);
            RunInterpreterCommand = new RelayCommand(RunInterpreter);
            SendInputCommand = new RelayCommand(SendInput);
            EndCurrentTaskCommand = new RelayCommand(EndCurrentInterpreterTask);
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

        private void RunInterpreter()
        {
            EndCurrentInterpreterTask();

            _embeddedInterpreter = new EmbeddedInterpreter(Document.Text);
            _embeddedInterpreter.NewOutputEvent += 
                new EventHandler<OutputEventArgs>(delegate (Object o, OutputEventArgs a) // TODO change to dispatcher
                {
                    if (a.Type == OutputType.Character)
                        Console.ConsoleText += a.CharacterOutput;
                    else
                        Console.ConsoleText += a.LineOutput + Environment.NewLine;
                });

            Console.ConsoleText = "";

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            var interpreterTask = Task.Factory.StartNew(() => 
                _embeddedInterpreter.StartExecuting(cancellationToken), cancellationToken);
        }

        private void SendInput()
        {
            Console.ConsoleText += ">"+Console.InputText + Environment.NewLine;
            _embeddedInterpreter.Input = Console.InputText + Environment.NewLine;
            _embeddedInterpreter.WaitHandle.Set();
            Console.InputText = "";
        }

        private void EndCurrentInterpreterTask()
        {
            _cancellationTokenSource?.Cancel();
            _embeddedInterpreter?.WaitHandle.Set();
        }
    }
}
