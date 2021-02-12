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

            //Check what flags have been passed
            foreach (var flag in allCompilerFlags)
            {
                if(args.Any(item => item == flag))
                {
                    compilerFlags.Add(flag);
                }
            }

            var sourceCode = File.ReadAllText(sourceFileName);

            var validateCode = new ValidateSourceCode(sourceCode);

            if(!validateCode.CheckIfCodeEndsWithEatAllOfThePancakesInstruction())
            {
                Console.WriteLine("The code must end with an \"Eat all of the pancakes!\" instruction");
                return;
            }

            var validationResult = validateCode.ValidateInstructions();

            if (validationResult.Item1)
            {
                PSCompiler.Compile(outputFileName, outputFileName + ".exe", validateCode.ValidSourceCode, compilerFlags);
                Console.WriteLine($"File: {sourceFileName} successfully compiled to {outputFileName}.exe");
            }
            else
            {
                Console.WriteLine($"Error: Instruction on line {validateCode.MapRealLineNumbersWithRaw()[validationResult.Item2] + 1} cannot be found");
            }
        }
    }
}
