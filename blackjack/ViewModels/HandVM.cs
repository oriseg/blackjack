using blackjack.ModelsLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace blackjack.ViewModels
{
    public class HandVM : ObservableObject
    {
        #region Fields
        private readonly Hand _hand;
        #endregion

        #region Properties
        public ObservableCollection<Card> Cards { get; }

        public int HandValue => _hand.HandValue;
        public Color HandColor => _hand.HandColor;
        public bool IsBust => _hand.IsBust;
        #endregion

        #region Constructor
        public HandVM(Hand hand)
        {
            _hand = hand;
            Cards = _hand.Cards; // ObservableCollection<Card>
        }
        #endregion
    }
}