using IDE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.ViewModels
{
    public class DebuggerViewModel
    {
        public DebuggerModel Debugger { get; set; }

        public DebuggerViewModel(DebuggerModel debugger)
        {
            Debugger = debugger;
        }


    }
}
