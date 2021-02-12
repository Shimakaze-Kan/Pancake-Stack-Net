using IDE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public class ReplaceViewModel
    {
        private DocumentModel _document;
        public ICommand ChangeVisiblilityOfReplaceWindowCommand { get; }
        public ICommand ReplaceTextCommand { get; }
        public ReplaceModel Replace { get; set; }

        public ReplaceViewModel(ReplaceModel replace, DocumentModel document)
        {
            Replace = replace;
            _document = document;

            ChangeVisiblilityOfReplaceWindowCommand = new RelayCommand(ChangeVisiblilityOfReplaceWindow);
            ReplaceTextCommand = new RelayCommand(ReplaceText, () => !string.IsNullOrEmpty(Replace.TextToReplace) && !string.IsNullOrEmpty(Replace.TextAfterReplace) && !string.IsNullOrEmpty(_document.Text));
        }

        private void ChangeVisiblilityOfReplaceWindow()
        {
            Replace.IsVisible = !Replace.IsVisible;
        }

        private void ReplaceText()
        {
            if (Replace.IsRegex)
            {
                _document.Text = Regex.Replace(_document.Text, Replace.TextToReplace, Replace.TextAfterReplace);
            }
            else
            {
                _document.Text = _document.Text.Replace(Replace.TextToReplace, Replace.TextAfterReplace);
            }

        }
    }
}
