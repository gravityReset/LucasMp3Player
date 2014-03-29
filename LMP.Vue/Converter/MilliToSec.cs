using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LMP.Vue.Converter
{
    class MilliToSec:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double nb = 0;
            double.TryParse(value.ToString(),out nb);
            return nb/1000;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int) (double.Parse(value.ToString())*1000);
            return val;
        }
    }
}
