using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LMP.Metier;

namespace LMP.Vue.Converter
{
    class IsInListToBoolConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var v = values[0] as IEnumerable<Chanson>;
            var song = values[1] as Chanson;
            if (v != null && song != null && v.Count() != 0)
                return v.All(c => c.Path != song.Path);

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
