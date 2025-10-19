﻿using blackjack.ModelsLogic;

namespace blackjack.Models
{
    abstract class UserModel
    {
        protected FbData fbd = new();
        public EventHandler? OnAuthComplete;
        public bool IsRegistered => !string.IsNullOrWhiteSpace(UserName);
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public abstract void Register();
        public abstract void Login(); 
        public abstract string GetFirebaseErrorMessage(string msg);
    }
}
