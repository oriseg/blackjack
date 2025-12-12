
using blackjack.Models;

namespace blackjack.ModelsLogic
{
    public class Card : CardModel
    {
        public Card(Shapes suit, Ranks rank, string ?imagePath)
        {
            Suit = suit;
            Rank = rank;
            ImagePath = imagePath;
        }
        public Card()
        {

        }
    }
}
