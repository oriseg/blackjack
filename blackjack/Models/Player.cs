namespace blackjack.Models
{
    internal class Player
    { 
        public string UserName { get; set; } = string.Empty; 
        internal Player(string username)
        {
            UserName= username;
        }
        internal Player()
        {

        }
    }
}
