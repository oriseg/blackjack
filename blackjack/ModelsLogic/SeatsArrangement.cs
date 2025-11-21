
using blackjack.Models;
using System.Collections.ObjectModel;

namespace blackjack.ModelsLogic
{
    public class SeatsArrangement
    {
        public void ArrangeSeats(ObservableCollection<Player> players, double width, double height)
        {
            double centerX = width / 2;
            double centerY = height * 0.55; // slightly below table center
            double radius = 200; // distance from table center

            int count = players.Count;
            double angleStep = 180.0 / (count + 1);

            for (int i = 0; i < count; i++)
            {
                // Semicircle from left to right (facing dealer at top)
                double angle = 0 + angleStep * (i + 1); // 0° = left, 180° = right
                double rad = angle * Math.PI / 180;

                // MAUI Y axis increases downward
                players[i].X = centerX + radius * Math.Cos(rad);
                players[i].Y = centerY + radius * Math.Sin(rad);
            }
        }
    }
}
