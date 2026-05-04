using blackjack.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Firebase.Auth;
using Plugin.CloudFirestore;

namespace blackjack.ModelsLogic
{
    public class User : UserModel
    {

        #region Constructor
        public User()
        {
            UserName = Preferences.Get(Keys.NameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            ProfileImage = Preferences.Get(Keys.ProfileImageKey, null);
        }
        #endregion

        #region Public Methods
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnCompleteReg);
        }

        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnCompleteLogin);
        }
        #endregion

        #region Private Methods
        private void OnCompleteReg(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                Coins = 1000;
                fbd.SetUserDocument(this, _ => { });
                SaveToPreferences();
                OnRegAuthComplete?.Invoke(this, EventArgs.Empty);
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

        private void OnCompleteLogin(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                fbd.GetDocument(Keys.UsersCollection, UserName, OnUserLoaded);
                this.IsRegistered = true;
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if (task.Exception?.InnerExceptions.Count > 0)
                {
                    if (task.Exception.InnerExceptions[0] is Firebase.Auth.FirebaseAuthHttpException)
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

        private void OnUserLoaded(IDocumentSnapshot? snapshot, Exception? error)
        {
            if (error != null)
            {
                // handle error if needed
                return;
            }
            if (snapshot != null && snapshot.Exists)
            {
                User? userFromDb = snapshot.ToObject<User>();
                Coins = userFromDb!.Coins;
            }

            OnAuthComplete?.Invoke(this, EventArgs.Empty);
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
            if(ProfileImage!=null)
                  Preferences.Set(Keys.ProfileImageKey, ProfileImage);
        }
        #endregion
    }
}