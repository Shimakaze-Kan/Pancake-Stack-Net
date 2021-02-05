using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Models
{
    public class CompilerFlagsModel : ObservableObject
    {
        private bool _waitFlag = true;

        public bool WaitFlag
        {
            get { return _waitFlag; }
            set { OnPropertyChanged(ref _waitFlag, value); }
        }

        private bool _noNewLineFlag;

        public bool NoNewLineFlag
        {
            get { return _noNewLineFlag; }
            set { _noNewLineFlag = value; }
        }

    }
}
