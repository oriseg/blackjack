using blackjack.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading.Tasks; 

namespace blackjack.ModelsLogic 
{
    internal class User : UserModel
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnCompleteReg);
        }

        private void OnCompleteReg(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SaveToPreferences();
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                ShowAlert(GetFirebaseErrorMessage(msg));

            } 
            else
            {
                ShowAlert(Strings.CreateUserError);

            }


        }


        public override string GetFirebaseErrorMessage(string msg)
        {
            if (msg.Contains(Strings.Reason))
            {
                if (msg.Contains(Strings.EmailExists))
                    return Strings.EmailExistsmsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    return Strings.InvalidEmailAddressmsg;
                if (msg.Contains(Strings.WeakPassword))
                    return Strings.WeakPasswordmsg;
                if (msg.Contains(Strings.UserNotFound))
                    return Strings.UserNotFoundmsg;
            }
            return Strings.UnknownError;
        }
        private static void ShowAlert(string msg)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }

        private void SaveToPreferences()
        {          
            Preferences.Set(Keys.NameKey, UserName);
            Preferences.Set(Keys.EmailKey, Email);
           
        }

        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnCompleteLogin); 
        }
        private void OnCompleteLogin(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                this.IsRegistered = true;
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowAlert(Strings.UserLoginError);
            }


        }
        public User()
        {
            UserName = Preferences.Get(Keys.NameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
         
        }
    }
}
