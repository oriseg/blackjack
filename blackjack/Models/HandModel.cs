using blackjack.ModelsLogic;

namespace blackjack.Models
{
    public abstract class HandModel
    {
        public int HandValue { get; protected set; }
        public Color HandColor { get; protected set; } = Colors.Black;
        public bool IsBust { get; set; }
        public int Total = 0;
        public abstract void AddCard(Card card);
        public abstract void Clear();
        public abstract void CaclculateHandValue();
        public EventHandler? OnHandValueChanged;
        public EventHandler? OnHandColorChanged;
        public EventHandler? OnHandStateChanged;
    }
}
