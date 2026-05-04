using blackjack.ModelsLogic;
using Plugin.CloudFirestore.Attributes;

namespace blackjack.Models
{
    public abstract class HandModel
    {
        #region Fields
        public int Total = 0;
        #endregion

        #region Properties
        public int HandValue { get; set; }
        public Color HandColor { get; set; } = Colors.Black;
        public bool IsBust { get; set; }
        #endregion

        #region Events
        [Ignored]
        public EventHandler? OnHandValueChanged;
        [Ignored]
        public EventHandler? OnHandColorChanged;
        [Ignored]
        public EventHandler? OnHandStateChanged;
        #endregion


        #region Public Methods
        public abstract void AddCard(Card card);
        public abstract void Clear();
        public abstract void CalculateHandValue();
        #endregion
    }
}