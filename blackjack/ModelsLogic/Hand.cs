
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
            IsBust = false;
            HandColor = Colors.Black; 
            OnHandColorChanged?.Invoke(this, EventArgs.Empty); 
            OnHandStateChanged?.Invoke(this, EventArgs.Empty); 
            OnHandValueChanged?.Invoke(this, EventArgs.Empty);
        }
        public override void CaclculateHandValue()
        {
            HandValue = Cards.Sum(card => card.GetCardValue()); 
            OnHandValueChanged?.Invoke(this, EventArgs.Empty);
            IsBust = HandValue > 21;
            if (IsBust)
                HandColor = Colors.Gray;
            else if (HandValue == 21)
                HandColor = Colors.Gold;
            else
                HandColor = Colors.Black;
            OnHandColorChanged?.Invoke(this, EventArgs.Empty);
            OnHandStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

