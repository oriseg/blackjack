
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
            OnHandValueChanged?.Invoke(this, EventArgs.Empty);
            OnHandColorChanged?.Invoke(this, EventArgs.Empty);
            OnHandStateChanged?.Invoke(this, EventArgs.Empty);
        }
        public int GetHandValue()
        {
            int total = 0;
            int aceCount = 0;

            foreach (Card card in Cards)
            {
                total += card.GetCardValue();

                if (card.Rank == Models.CardModel.Ranks.Ace)
                    aceCount++;
            }

            // Downgrade Aces from 11 to 1 as needed
            while (total > 21 && aceCount > 0)
            {
                total -= 10; // 11 -> 1
                aceCount--;
            }

            return total;
        }

        public override void CalculateHandValue()
        {
            HandValue = GetHandValue();
            IsBust = HandValue > 21;
            if (IsBust)
                HandColor = Colors.Gray;
            else if (HandValue == 21)
                HandColor = Colors.Gold;
            else
                HandColor = Colors.Black;
            OnHandValueChanged?.Invoke(this, EventArgs.Empty);
            OnHandColorChanged?.Invoke(this, EventArgs.Empty);
            OnHandStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
