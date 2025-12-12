
using System.Collections.ObjectModel;

namespace blackjack.ModelsLogic
{
    public class Hand
    {
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        public void Clear()
        {
            Cards.Clear();     
        }
    }
}
