using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Models
{
    public class ReplaceModel : ObservableObject
    {
        private string _textToReplace;

        public string TextToReplace
        {
            get { return _textToReplace; }
            set { OnPropertyChanged(ref _textToReplace, value); }
        }

        private bool _isRegex;

        public bool IsRegex
        {
            get { return _isRegex; }
            set { OnPropertyChanged(ref _isRegex, value); }
        }

        private string _textAfterReplace;

        public string TextAfterReplace
        {
            get { return _textAfterReplace; }
            set { OnPropertyChanged(ref _textAfterReplace, value); }
        }

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set { OnPropertyChanged(ref _isVisible, value); }
        }

    }
}
