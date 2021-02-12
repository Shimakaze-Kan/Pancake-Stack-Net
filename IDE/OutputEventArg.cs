using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    public enum OutputType
    {
        Character,
        Line
    }

    /// <summary>
    /// Helper class to determine what type of data is being sent
    /// </summary>
    public class OutputEventArgs : EventArgs
    {
        public OutputType Type { get; set; }
        public char CharacterOutput { get; set; }
        public string LineOutput { get; set; }
        public bool RuntimeError { get; set; }
        public int LineNumberErrorHandling { get; set; }
    }
}
