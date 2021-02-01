using IDE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public class CompilerViewModel
    {
        public ICommand CompileCommand { get; set; }

        private readonly DocumentModel _document;
        private readonly ConsoleModel _console;

        public CompilerViewModel(DocumentModel document, ConsoleModel console)
        {
            _document = document;
            _console = console;

            CompileCommand = new RelayCommand(Compile, () => !string.IsNullOrEmpty(_document.FilePath) && !string.IsNullOrEmpty(_document.FileName));
        }

        private void Compile()
        {
            Process process = new Process();
            process.StartInfo.FileName = "PancakeStack_Compiler.exe";
            process.StartInfo.Arguments = string.Format("{0} {1}", _document.FilePath, Path.GetFileNameWithoutExtension(_document.FileName));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += new DataReceivedEventHandler((_, e) => _console.ConsoleText += e.Data + Environment.NewLine);
            //process.ErrorDataReceived += new DataReceivedEventHandler((_, e) => _console.ConsoleText += "Error" + e.Data + Environment.NewLine);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
