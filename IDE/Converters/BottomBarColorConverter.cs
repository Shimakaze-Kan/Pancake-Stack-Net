using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace IDE.Converters
{
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class BottomBarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#3A66A7");
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FF9912");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
