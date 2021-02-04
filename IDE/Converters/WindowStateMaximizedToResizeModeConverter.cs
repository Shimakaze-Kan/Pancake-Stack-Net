using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IDE.Converters
{
    [ValueConversion(typeof(WindowState), typeof(ResizeMode))]
    class WindowStateMaximizedToResizeModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (WindowState)value == WindowState.Maximized ? ResizeMode.NoResize : ResizeMode.CanResizeWithGrip;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
