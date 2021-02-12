using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IDE.Models
{
    public class DebugDocumentModel
    {
        private readonly SolidColorBrush SolidColorBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#FFEC8B");

        private int _linePointer;

        public int LinePointer
        {
            get { return _linePointer; }
            set 
            {
                if (value < Lines.Count)
                {
                    _linePointer = value;
                    Lines[_linePointer].BackgroundColor = SolidColorBrush;
                    Lines[_linePointer].ForegroundColor = Brushes.Black;
                }
            }
        }

        public ObservableCollection<TextLine> Lines { get; set; }
    }
}
