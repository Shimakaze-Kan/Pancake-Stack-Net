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
        private FontFamily _family;

        public FontFamily Family
        {
            get { return _family; }
            set { OnPropertyChanged(ref _family, value); }
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
