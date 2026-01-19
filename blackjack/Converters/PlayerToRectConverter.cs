using blackjack.ViewModels; // כדי לזהות PlayerVM
using System.Globalization;

namespace blackjack.Converters
{
    public class PlayerToRectConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is PlayerVM playerVM)
            {
                double size = 60;
                return new Microsoft.Maui.Graphics.Rect(playerVM.X - size / 2, playerVM.Y - size / 2, size, size);
            }

            return new Microsoft.Maui.Graphics.Rect(0, 0, 60, 60);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
