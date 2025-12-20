
using System.ComponentModel;

namespace blackjack.ModelsLogic
{
    public class Dealer
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Hand _dealerHand = new Hand();
        public Hand DealerHand
        {
            get => _dealerHand;
            set
            {
                _dealerHand = value;
                OnPropertyChanged(nameof(DealerHand));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
