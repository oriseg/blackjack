using blackjack.ModelsLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace blackjack.ViewModels
{
    public class HandVM : ObservableObject
    {
        private readonly Hand _hand;

        public HandVM(Hand hand)
        {
            _hand = hand;
            Cards = _hand.Cards; // ObservableCollection<Card>
        }

        public ObservableCollection<Card> Cards { get; }

        public int HandValue => _hand.HandValue;
        public Color HandColor => _hand.HandColor;
        public bool IsBust => _hand.IsBust;
    }
}
