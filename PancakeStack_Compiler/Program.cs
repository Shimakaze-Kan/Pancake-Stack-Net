﻿using System;
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
            string sourceFileName = args[0];
            string outputFileName = args[1];
            bool compilerFlag = false;
            if(args.Length == 3 && args[2] == "-wait")
            {
                compilerFlag = true;
            }

            var sourceCode = File.ReadAllLines(sourceFileName);

            //sourceCode = sourceCode.Select(item => item.Trim());

            PSCompiler.Compile(outputFileName, outputFileName + ".exe", sourceCode, compilerFlag);
            Console.WriteLine($"File: {sourceFileName} successfully compiled to {outputFileName}.exe");
        }
    }
}