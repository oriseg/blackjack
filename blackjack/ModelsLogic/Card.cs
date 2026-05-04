using blackjack.Models;

namespace blackjack.ModelsLogic
{
    public class Card : CardModel
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public Card(Shapes suit, Ranks rank, string? imagePath, bool isFaceDown = false)
        {
            Suit = suit;
            Rank = rank;
            ImagePath = imagePath;
            IsFaceDown = isFaceDown;
        }

        public Card()
        {
        }
        #endregion

        #region Public Methods
        public override int GetCardValue()
        {
            return Rank switch
            {
                Ranks.Ace => 11,
                Ranks.Two => 2,
                Ranks.Three => 3,
                Ranks.Four => 4,
                Ranks.Five => 5,
                Ranks.Six => 6,
                Ranks.Seven => 7,
                Ranks.Eight => 8,
                Ranks.Nine => 9,
                Ranks.Ten or Ranks.Jack or Ranks.Queen or Ranks.King => 10,
                _ => 0,
            };
        }
        #endregion

        #region Private Methods
        #endregion
    }
}