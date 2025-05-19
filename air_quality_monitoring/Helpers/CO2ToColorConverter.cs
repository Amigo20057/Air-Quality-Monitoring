using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace air_quality_monitoring
{
    public class CO2ToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double co2)
            {
                if (co2 <= 50)
                    return Brushes.Green;
                else if (co2 <= 100)
                    return Brushes.Yellow;
                else if (co2 <= 150)
                    return Brushes.Orange;
                else if (co2 <= 200)
                    return Brushes.Red;
                else
                    return Brushes.Purple;
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}