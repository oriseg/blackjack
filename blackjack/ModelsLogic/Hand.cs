
using blackjack.Models;
using System.Collections.ObjectModel;

namespace blackjack.ModelsLogic
{
    public class Hand : HandModel
    {
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public override void AddCard(Card card)
        {
            Cards.Add(card);
            CaclculateHandValue();
        }
        public override void Clear()
        {
            Cards.Clear();
            Isbust = false;
            HandColor = Colors.Black;
        }
        public override void CaclculateHandValue()
        {
            HandValue = Cards.Sum(card => card.GetCardValue());
            Isbust = HandValue > 21;
            if (Isbust)
                HandColor = Colors.Gray;
            else if (HandValue == 21)
                HandColor = Colors.Gold;
            else
                HandColor = Colors.Black;

        }
    }
}

