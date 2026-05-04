using blackjack.ModelsLogic;
using Plugin.CloudFirestore.Attributes;

namespace blackjack.Models
{
    public abstract class UserModel
    {
        #region Fields
        protected FbData fbd = new();
        #endregion

        #region Events
        public EventHandler? OnAuthComplete;
        [Ignored]
        public EventHandler? OnRegAuthComplete;
        #endregion

        #region Properties
        public int Coins { get; set; } = 1000;
        public bool IsRegistered { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        #endregion

        #region Public Methods
        public abstract void Register();
        public abstract void Login();
        #endregion

    }
}