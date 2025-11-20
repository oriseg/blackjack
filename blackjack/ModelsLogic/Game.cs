
using blackjack.Models;
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
            Player joinedPlayer = new Player(HostName);
            fbd.UpdateFields(Keys.GamesCollection, GameCode, "Players", FieldValue.ArrayUnion(joinedPlayer), OnComplete);
           
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
                    this.Players = game.Players;
                    this.Created = game.Created;
                }

       
            }
            OnGameJoined?.Invoke(this, EventArgs.Empty);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            bool gameChanged = false;
            if (updatedGame != null)
            {
                if(Players.Count != updatedGame.Players.Count)
                {
                    Players = updatedGame.Players;
                    IsFull = updatedGame.IsFull;
                    gameChanged = true;
                }
                if(gameChanged)
                {
                    OnGameChanged?.Invoke(this, true);
                }
                
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
