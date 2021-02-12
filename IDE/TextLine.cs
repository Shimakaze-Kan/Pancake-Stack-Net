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
        private SolidColorBrush _backgroudColor;

        public SolidColorBrush BackgroundColor
        {
            get { return _backgroudColor; }
            set { OnPropertyChanged(ref _backgroudColor, value); }
        }

        private SolidColorBrush _foregroundColor = Brushes.White;

        public SolidColorBrush ForegroundColor
        {
            get { return _foregroundColor; }
            set { OnPropertyChanged(ref _foregroundColor, value); }
        }

    }
}
