using blackjack.ModelsLogic;

namespace blackjack.Models
{
    public abstract class HandModel
    { 
       public int HandValue { get; set; }
       public abstract void AddCard(Card card);  
       public abstract void Clear(); 
       public abstract void CaclculateHandValue();


    }
}
