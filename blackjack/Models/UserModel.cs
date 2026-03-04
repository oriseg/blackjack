using blackjack.ModelsLogic;
using Plugin.CloudFirestore.Attributes;


namespace blackjack.Models
{
    public abstract class UserModel
    {

        protected FbData fbd = new(); 

        public EventHandler? OnAuthComplete;
        [Ignored]
        public EventHandler? OnRegAuthComplete;
        public int Coins { get; set; } = 1000;
        public bool IsRegistered { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ProfileImagePath { get;  set; }
        public abstract void Register();
        public abstract void Login(); 
      

    }
}
