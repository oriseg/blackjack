
using blackjack.Models;
using Microsoft.Maui.Storage;
using Plugin.CloudFirestore;

namespace blackjack.ModelsLogic
{
    public class Game : GameModel
    {
        public Game()
        {
            HostName = new User().UserName; 
            Created = DateTime.Now; 
            
        }
        public Game(int playercount)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            PlayerCount = playercount;

        }
        public void createGame(int PlayerCount)
        {
            //Game game = new Game(PlayerCount); 
            //game.Players.Add(new Player(HostName));
            //game.SetDocument(OnComplete);
            this.PlayerCount = PlayerCount;
            this.Players.Add(new Player(HostName));
            this.SetDocument(OnComplete); 

        } 
        private void OnComplete(Task task)
        {  
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
        }
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete); 
        } 
        public void joinGame(string GameCode)
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
        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                IsFull = updatedGame.IsFull;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            DeleteDocument(OnComplete);
        }
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
    }
}
