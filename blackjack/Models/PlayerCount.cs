
using Plugin.CloudFirestore.Attributes;

namespace blackjack.Models
{
    public class PlayerCount
    { 
        public int Count { get; set; }
        [Ignored]
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
