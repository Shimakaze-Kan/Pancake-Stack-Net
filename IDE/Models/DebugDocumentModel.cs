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
        private int _linePointer;

        public int LinePointer
        {
            get { return _linePointer; }
            set 
            {
                if (value < Lines.Count)
                {
                    _linePointer = value;
                    Lines[_linePointer].BackgroundColor = Brushes.Yellow;
                }
            }
        }

        public ObservableCollection<TextLine> Lines { get; set; }
    }
}
