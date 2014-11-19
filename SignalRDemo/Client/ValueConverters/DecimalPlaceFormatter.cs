using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Client.ValueConverters
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class DoubleToStringConverter : IValueConverter
    {
        private DoubleToStringConverter()
        {
            
        }
        static DoubleToStringConverter()
        {
            Instance = new DoubleToStringConverter();
        }

        public static DoubleToStringConverter Instance { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : ((decimal)value).ToString("#,0.##");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal retValue;
            if (decimal.TryParse(value as string, out retValue))
            {
                return retValue;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
