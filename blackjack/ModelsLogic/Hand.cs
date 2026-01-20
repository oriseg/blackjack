using blackjack.Models;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace blackjack.ModelsLogic
{
    public class Hand : HandModel
    {
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public override void AddCard(Card card)
        {
            Cards.Add(card);
            CalculateHandValue();
        }

        public override void Clear()
        {
            Cards.Clear();
            IsBust = false;
            HandColor = Colors.Black;
        }

        public override void CalculateHandValue()
        {
            HandValue = Cards.Sum(card => card.GetCardValue());
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
