using Plugin.CloudFirestore.Attributes;
namespace blackjack.Models
{
    public class Player 
    { 
        public string UserName { get; set; } = string.Empty;
        public bool IsCurrentTurn { get; set; }
        [Ignored]
        public double X { get; set; }
        [Ignored]
        public double Y { get; set; }
        public Player(string username)
        {
            UserName= username;
        }
        public Player()
        {

        }
   
    }
}
