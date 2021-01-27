using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IDE.Models
{
    public class FormatModel : ObservableObject
    {
        private FontStyle _style;

        public FontStyle Style
        {
            get { return _style; }
            set { OnPropertyChanged(ref _style, value); }
        }

        private FontWeight _weight;

        public FontWeight Weight
        {
            get { return _weight; }
            set { OnPropertyChanged(ref _weight, value); }
        }

        private FontFamily _family;

        public FontFamily Family
        {
            get { return _family; }
            set { _family = value; }
        }

        private TextWrapping _wrap;

        public TextWrapping Wrap
        {
            get { return _wrap; }
            set 
            { 
                OnPropertyChanged(ref _wrap, value);
                _isWrapped = value == TextWrapping.Wrap;
            }
        }

        private bool _isWrapped;

        public bool IsWrapped
        {
            get { return _isWrapped; }
            set { OnPropertyChanged(ref _isWrapped, value); }
        }

        private double _size;

        public double Size
        {
            get { return _size; }
            set { OnPropertyChanged(ref _size, value); }
        }
    }
}
