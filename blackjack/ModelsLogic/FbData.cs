using blackjack.Models;
using blackjack.Views;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Newtonsoft.Json.Linq;
using Plugin.CloudFirestore;
using System.Text.RegularExpressions;


namespace blackjack.ModelsLogic
{
    public class FbData:FbDataModel
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
                if (facl.User != null)
                {
                    Preferences.Set(Keys.NameKey, facl.User.Info.DisplayName);
                    Preferences.Set(Keys.EmailKey, facl.User.Info.Email);
                }
             

            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", Strings.UserLoginError, "OK");
            }
            
        }
        public override string GetFirebaseErrorMessage(string errMessage)
        {
            string retMessage;
            int end, start = errMessage.IndexOf(Keys.MessageKey);
            if (start > 0)
            {
                end = errMessage.IndexOf(Keys.ErrorsKey, start);

                string title = errMessage[(start + Keys.MessageKey.Length)..end]
                    .Replace(Keys.Apostrophe, string.Empty)
                    .Replace(Keys.Colon, string.Empty)
                    .Replace(Keys.Comma, string.Empty)
                    .Replace("\"", string.Empty)
                    .Trim();
                title = string.Join(Keys.WordsDelimiter, title.Split(Keys.TitleDelimiter));
                string reason = errMessage[(errMessage.IndexOf(Keys.ReasonKey) +
                    Keys.ReasonKey.Length)..]
                    .Replace(Keys.Apostrophe, string.Empty)
                    .Replace(Keys.Colon, string.Empty)
                    .Replace(Keys.Comma, string.Empty)
                    .Replace("\"", string.Empty)
                    .Replace(Keys.NewLine, string.Empty)
                    .Replace("}", string.Empty)
                    .Replace("]", string.Empty)
                    .Trim();
                
                
                retMessage = title + Keys.NewLine + Keys.ReasonKey +
                Keys.WordsDelimiter + reason;
            }
            else
                retMessage = errMessage;
            return retMessage;
        }
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fdb.Collection(collectonName).Document() : fdb.Collection(collectonName).Document(id); 
            ((Game)obj).Id = dr.Id;
            dr.SetAsync(obj).ContinueWith(OnComplete); 
            return dr.Id;
        }
        public async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        public override IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }

        public override async void UpdateFields(string collectonName, string id, string fieldName,FieldValue fieldValue, Action<IQuerySnapshot> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.UpdateAsync(fieldName, fieldValue);
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(Strings.Id, id).GetAsync();
            OnComplete(qs);
        }
        public override async void UpdateFields(string collectonName, string id, string fieldName, object value, Action<IQuerySnapshot> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.UpdateAsync(fieldName, value);
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(Strings.Id, id).GetAsync();
            OnComplete(qs);
        }
        public override IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fdb.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
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
