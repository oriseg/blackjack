using blackjack.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Firebase.Auth;


namespace blackjack.ModelsLogic 
{
    public class User : UserModel
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
                ShowAlert(fbd.GetFirebaseErrorMessage(msg));

            } 
            else
            {
                ShowAlert(Strings.CreateUserError);

            }


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
                if (task.Exception?.InnerExceptions.Count >0)
                { 
                    if(task.Exception.InnerExceptions[0] is Firebase.Auth.FirebaseAuthHttpException)
                    {
                       string msg = ((FirebaseAuthHttpException)task.Exception.InnerExceptions[0]).ResponseData;
                        ShowAlert(fbd.GetFirebaseErrorMessage(msg));
                    }
                    else
                    {
                        ShowAlert(Strings.UserLoginError);
                    }

                }      
            }
        }
        public User()
        {
            UserName = Preferences.Get(Keys.NameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            ProfileImagePath = Preferences.Get(Keys.ProfileImageKey, null);
        }
        public async Task PickProfileImageAsync()
        {
            var path = await mediaService!.PickImageAsync();
            if (path == null) return;

            ProfileImagePath = path;
        }

        public async Task TakePhotoAsync()
        {
            var path = await mediaService!.TakePhotoAsync();
            if (path == null) return;
            ProfileImagePath = path;
        }
    }
}
