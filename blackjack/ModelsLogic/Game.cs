
using blackjack.Models;
using Microsoft.Maui.Storage;
using Plugin.CloudFirestore;

namespace blackjack.ModelsLogic
{
    internal class Game : GameModel
    { 
        internal Game()
        {
            HostName = new User().UserName; 
            Created = DateTime.Now; 
            
        }
        internal Game(int playercount)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            PlayerCount = playercount;

        }
        internal void createGame(int PlayerCount)
        {
            Game game = new Game(PlayerCount); 
            game.Players.Add(new Player(HostName));
            game.SetDocument(OnComplete);
        }
        private void OnComplete(Task task)
        {
         
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
        }
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete); 
        } 
        internal void joinGame(string GameCode)
        { 

            Game game = new Game();
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, Strings.Id, GameCode, OnComplete);
        }
        private void OnComplete(IQuerySnapshot qs)
        {
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    //if game not full
                    //if username not exist in list

                    IDocumentReference dr = CrossCloudFirestore.Current.Instance.Collection(Keys.GamesCollection).Document(ds.Id);
                    dr.UpdateAsync("Players", FieldValue.ArrayUnion(new Player(HostName)));
                }

       
            }
        }
    }
}
