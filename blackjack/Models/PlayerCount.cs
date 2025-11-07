
namespace blackjack.Models
{
    class PlayerCount
    { 
        public int Count { get; set; }
        public string DisplayName => $"{Count}";
        public PlayerCount(int count)
        {
            Count = count;
        } 
        public PlayerCount()
        {
            
        }
    }
}
