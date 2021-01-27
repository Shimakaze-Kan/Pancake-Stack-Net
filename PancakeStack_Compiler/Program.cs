using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PancakeStack_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Error: Not enough input arguments");
                return;
            }

            string sourceFileName = args[0];
            string outputFileName = args[1];

            string[] allCompilerFlags = new string[] { "-wait", "-nonewline" };
            List<string> compilerFlags = new List<string>();

            foreach (var flag in allCompilerFlags)
            {
                if(args.Any(item => item == flag))
                {
                    compilerFlags.Add(flag);
                }
            }

            var sourceCode = File.ReadAllLines(sourceFileName);

            //sourceCode = sourceCode.Select(item => item.Trim());

            PSCompiler.Compile(outputFileName, outputFileName + ".exe", sourceCode, compilerFlags);
            Console.WriteLine($"File: {sourceFileName} successfully compiled to {outputFileName}.exe");
        }
    }
}
