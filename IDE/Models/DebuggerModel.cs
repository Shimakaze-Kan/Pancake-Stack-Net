using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Models
{
    public class DebuggerModel : ObservableObject
    {
        private List<int> _stack;

        public List<int> Stack
        {
            get { return _stack; }
            set 
            {
                OnPropertyChanged(ref _stack, value);
            }
        }

        private List<KeyValuePair<string,int>> _label;

        public List<KeyValuePair<string,int>> Label
        {
            get { return _label; }
            set 
            {
                OnPropertyChanged(ref _label, value); 
            }
        }

    }
}
