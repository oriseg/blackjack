using blackjack.ModelsLogic;
using System.Collections.ObjectModel;

public abstract class HandModel
{
    public int HandValue { get; protected set; }
    public Color HandColor { get; protected set; } = Colors.Black;

    public int Total { get; protected set; }
    public bool IsBust { get; protected set; }

    public ObservableCollection<Card> Cards { get; } = new();

    public abstract void AddCard(Card card);
    public abstract void Clear();
    public abstract void CalculateHandValue();
}