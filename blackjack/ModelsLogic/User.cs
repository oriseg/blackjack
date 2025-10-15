using blackjack.Models;
namespace blackjack.ModelsLogic
{
    internal class User : UserModel
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                SaveToPreferences();
            else
                Shell.Current.DisplayAlert(Strings.CreateUserError, task.Exception?.Message, Strings.Ok);
        }

        private void SaveToPreferences()
        {
            Preferences.Set(Keys.NameKey, UserName);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.PasswordKey, Password);
        }

        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnComplete); 
        }

        public User()
        {
            UserName = Preferences.Get(Keys.NameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
        }
    }
}
