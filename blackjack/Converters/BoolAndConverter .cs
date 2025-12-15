using System.Globalization;

namespace blackjack.Converters
{

        public class BoolAndConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                return values.All(v => v is bool b && b);
            }

            public object[] ?ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
                => null;
        }
    
}
