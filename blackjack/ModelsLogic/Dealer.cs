namespace blackjack.ModelsLogic
{
    public class Dealer
    {
        #region Fields
        private Hand _dealerHand = new Hand();
        #endregion

        #region Properties
        public Hand DealerHand
        {
            get => _dealerHand;
            set
            {
                _dealerHand = value;
            }
        }
        #endregion

    }
}