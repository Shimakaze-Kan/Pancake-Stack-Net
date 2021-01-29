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
        public FormatModel Format { get; set; }
        public DocumentModel Document { get; set; }
        public ConsoleModel Console { get; set; }

        private EmbeddedInterpreter embeddedInterpreter;

        public EditorViewModel(DocumentModel document)
        {
            Console = new ConsoleModel(); // tmp
            Document = document;
            Format = new FormatModel() { Family = new FontFamily("Consolas"), Size = 18 };
            FormatCommand = new RelayCommand(OpenFormatDialog);
            WrapCommand = new RelayCommand(ToggleWrap);
            RunInterpreterCommand = new RelayCommand(RunInterpreter);
            SendInputCommand = new RelayCommand(SendInput);
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
            embeddedInterpreter = new EmbeddedInterpreter(Document.Text);
            embeddedInterpreter.NewOutputEvent += 
                new EventHandler<OutputEventArgs>(delegate (Object o, OutputEventArgs a) // TODO change to dispatcher
                {
                    if (a.Type == OutputType.Character)
                        Console.ConsoleText += a.CharacterOutput;
                    else
                        Console.ConsoleText += a.LineOutput + Environment.NewLine;
                });

            Console.ConsoleText = "";

            CancellationToken interpreterCancellationToken;
            var interpreterTask = Task.Factory.StartNew(() => 
                embeddedInterpreter.StartExecuting(interpreterCancellationToken), interpreterCancellationToken);
        }

        private void SendInput()
        {
            embeddedInterpreter.Input = Console.InputText + Environment.NewLine;
            embeddedInterpreter.WaitHandle.Set();
            Console.InputText = "";
        }
    }
}
