using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;
using blackjack.Models;
using blackjack.Views;

namespace blackjack.ModelsLogic
{
    class FbData:FbDataModel
    {   
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            try
            {
                await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
             

            }
            catch (Exception)
            {

                    await Shell.Current.DisplayAlert("Error", Strings.CreateUserError, "OK");  
            }
        }
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            try
            {
                await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
            
               

            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", Strings.UserLoginError, "OK");
            }
            
        }
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fdb.Collection(collectonName).Document() : fdb.Collection(collectonName).Document(id); 
            ((Game)obj).Id = dr.Id;
            dr.SetAsync(obj).ContinueWith(OnComplete); 
            return dr.Id;
        }
        public override string DisplayName
        {
            get
            {
                string dn = string.Empty;
                if (facl.User != null)
                    dn = facl.User.Info.DisplayName;
                return dn;
            }
        }
        public override string UserId
        {
            get
            {
                return facl.User.Uid;
            }
        }
    }
}
