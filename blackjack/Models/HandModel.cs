using blackjack.ModelsLogic;
using Plugin.CloudFirestore.Attributes;
public abstract class HandModel
    {
        public int HandValue { get; set; }
        public Color HandColor { get; set; } = Colors.Black;
        public bool IsBust { get; set; }
        public int Total = 0;
        public abstract void AddCard(Card card);
        public abstract void Clear();
        public abstract void CalculateHandValue();
    [Ignored]
        public EventHandler? OnHandValueChanged;
    [Ignored]
    public EventHandler? OnHandColorChanged;
    [Ignored]
    public EventHandler? OnHandStateChanged;
    }

