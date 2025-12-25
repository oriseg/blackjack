
using blackjack.Models;
using System.Collections.ObjectModel;

namespace blackjack.ModelsLogic
{
    public class Hand : HandModel
    {
        public ObservableCollection<Card> Cards { get; set; } = [];

        public override void AddCard(Card card)
        {
            Cards.Add(card);
        }
        public override void Clear()
        {
            Cards.Clear();
        }
        public override void CaclculateHandValue()
        {
            foreach (Card card in Cards)
            {
                HandValue += card.GetCardValue();
            }
        }
    }
}
