

using blackjack.Models;
using System.Globalization;

namespace blackjack.Converters
{
    public class PlayerToRectConverter : IValueConverter
    {

            public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            {
            if (value is Player player)
            {
                double size = 60;
                return new Microsoft.Maui.Graphics.Rect(player.X - size / 2, player.Y - size / 2, size, size);
            }
            return new Microsoft.Maui.Graphics.Rect(0, 0, 60, 60);
        }

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            {
            return null;
            }
               
    }
}
