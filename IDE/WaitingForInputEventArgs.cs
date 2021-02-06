using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    public enum InputType
    {
        Alphanumeric,
        Numeric
    }
    public class WaitingForInputEventArgs : EventArgs
    {
        public InputType Type { get; set; }
    }
}
