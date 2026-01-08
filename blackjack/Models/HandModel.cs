using blackjack.ModelsLogic;

namespace blackjack.Models
{
    public abstract class HandModel : ObservableObject
    {
        // OnPropertyChanged in the model hare temporary just to see how it looks like in the UI
        private int _handValue = 0;
        public int HandValue
        {
            get => _handValue;
            set
            {
                if (_handValue != value)
                {
                    _handValue = value;
                    OnPropertyChanged();
                }
            }
        }
        private Color _handColor = Colors.Black;
        public Color HandColor
        {
            get => _handColor;
            protected set
            {
                if (_handColor != value)
                {
                    _handColor = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Total = 0;
        public bool Isbust { get; protected set; } = false;
        public abstract void AddCard(Card card);
        public abstract void Clear();
        public abstract void CaclculateHandValue();


    }
}
