namespace blackjack.Models
{
    public class Player
    { 
        public string UserName { get; set; } = string.Empty; 
        public Player(string username)
        {
            UserName= username;
        }
        public Player()
        {

        }
    }
}
