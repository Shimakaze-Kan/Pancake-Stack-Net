using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Models
{
    public class ConsoleModel : ObservableObject
    {
        //public class StringWrapper
        //{
        //    public string Value { get; set; }
        //}

        //private ObservableCollection<StringWrapper> _buffer;

        //public ObservableCollection<StringWrapper> Buffer
        //{
        //    get { return _buffer; }
        //    set 
        //    {
        //        OnPropertyChanged(ref _buffer, value);
        //    }
        //}

        private string _consoleText;

        public string ConsoleText
        {
            get { return _consoleText; }
            set 
            {
                OnPropertyChanged(ref _consoleText, value); 
            }
        }

        private string _inputText;

        public string InputText
        {
            get { return _inputText; }
            set 
            {
                OnPropertyChanged(ref _inputText, value);
            }
        }


    }
}
