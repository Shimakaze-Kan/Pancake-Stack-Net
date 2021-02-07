using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IDE.Converters
{
    [ValueConversion(typeof(List<KeyValuePair<string, Tuple<int, int>>>), typeof(int))]
    public class ListKeyValuePairTupleToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return 0;

            return ((List<KeyValuePair<string, Tuple<int, int>>>)value).Count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
