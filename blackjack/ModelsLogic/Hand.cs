using blackjack.Models;
using Microsoft.Maui.Graphics;

namespace blackjack.ModelsLogic
{
    public class Hand : HandModel
    {
        public override void AddCard(Card card)
        {
            Cards.Add(card);
            CalculateHandValue();
        }

        public override void Clear()
        {
            Cards.Clear();
            Total = 0;
            HandValue = 0;
            IsBust = false;
            HandColor = Colors.Black;
        }

        public override void CalculateHandValue()
        {
            Total = Cards.Sum(card => card.GetCardValue());
            HandValue = Total;

            IsBust = HandValue > 21;

            if (IsBust)
                HandColor = Colors.Gray;
            else if (HandValue == 21)
                HandColor = Colors.Gold;
            else
                HandColor = Colors.Black;
        }
    }
}
