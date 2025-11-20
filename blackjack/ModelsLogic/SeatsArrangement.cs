
using blackjack.Models;
using System.Collections.ObjectModel;

namespace blackjack.ModelsLogic
{
    public class SeatsArrangement
    {
        public void ArrangeSeats(ObservableCollection<Player> players, double width, double height)
        {
            double centerX = width / 2;
            double centerY = height * 0.55;
            double radius = 200;

            int count = players.Count;
            double angleStep = 180.0 / (count + 1);

            for (int i = 0; i < count; i++)
            {
                double angle = 180 + angleStep * (i + 1);
                double rad = angle * Math.PI / 180;

                players[i].X = centerX + radius * Math.Cos(rad);
                players[i].Y = centerY + radius * Math.Sin(rad);
            }
        }
    }
}
