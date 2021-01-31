using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IDE
{
    public class TextLine : ObservableObject
    {
        public string Text { get; set; }
        private SolidColorBrush _color;

        public SolidColorBrush BackgroundColor
        {
            get { return _color; }
            set { OnPropertyChanged(ref _color, value); }
        }
    }
}
