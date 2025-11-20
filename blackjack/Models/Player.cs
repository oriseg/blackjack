namespace blackjack.Models
{
    public class Player
    { 
        public string UserName { get; set; } = string.Empty;
        public double X { get; set; }
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
