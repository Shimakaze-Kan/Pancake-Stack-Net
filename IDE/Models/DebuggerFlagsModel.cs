using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Models
{
    public class DebuggerFlagsModel : ObservableObject
    {
        private bool _noNewLineFlag;

        public bool NoNewLineFlag
        {
            get { return _noNewLineFlag; }
            set { OnPropertyChanged(ref _noNewLineFlag, value); }
        }

        private bool _noInputInConsoleFlag;

        public bool NoInputInConsoleFlag
        {
            get { return _noInputInConsoleFlag; }
            set { OnPropertyChanged(ref _noInputInConsoleFlag, value); }
        }
    }
}
