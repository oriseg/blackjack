using blackjack.Models;
using System.Globalization;

namespace blackjack.Converters
{
    public class PlayerPopupRectConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var player = value as Player;
            if (player == null) return new Rect(0, 0, 200, 50);

            // Position the popup just below the player
            double width = 200;
            double height = 50;
            return new Rect(player.X - width / 2, player.Y + 50, width, height);
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}

