
using System.ComponentModel;

namespace blackjack.ModelsLogic
{
    public class Dealer
    {
        private Hand _dealerHand = new Hand();
        public Hand DealerHand
        {
            get => _dealerHand;
            set
            {
                _dealerHand = value;

            }
        }
    }
}
