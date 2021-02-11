using IDE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public class CompilerViewModel
    {
        public ICommand CompileCommand { get; set; }

        private readonly DocumentModel _document;
        private readonly ConsoleModel _console;

        public CompilerFlagsModel CompilerFlags { get; set; }

        public CompilerViewModel(DocumentModel document, ConsoleModel console, CompilerFlagsModel compilerFlags)
        {
            CompilerFlags = compilerFlags;

            _document = document;
            _console = console;

            CompileCommand = new RelayCommand(Compile, () => !string.IsNullOrEmpty(_document.FilePath) && !string.IsNullOrEmpty(_document.FileName));
        }

        private void Compile()
        {
            if(!File.Exists("PancakeStack_Compiler.exe"))
            {
                MessageBox.Show("Compiler not found, make sure it exists in the same folder as the IDE and is not occupied by another process",
                    "Compiler does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<string> flags = new List<string>();
            if (CompilerFlags.WaitFlag)
                flags.Add("-wait");
            if (CompilerFlags.NoNewLineFlag)
                flags.Add("-nonewline");

            string arguments = string.Format("{0} {1} {2}", _document.FilePath, Path.GetFileNameWithoutExtension(_document.FileName), string.Join(" ", flags));

            Process process = new Process();
            process.StartInfo.FileName = "PancakeStack_Compiler.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += new DataReceivedEventHandler((_, e) => _console.ConsoleText += e.Data + Environment.NewLine);            

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
