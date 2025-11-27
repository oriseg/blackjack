
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
            //clean prev game after back
            this.Id= string.Empty;
            this.Players.Clear();

            this.PlayerCount = PlayerCount;
            this.Players.Add(new Player(HostName));
            this.SetDocument(OnComplete); 

        }
        public override void ArrangePlayerSeats()
        {
            double width = Keys.Width;
            double height = Keys.Length;
            ArrangeSeats(width, height);

        }
        public void ArrangeSeats(double width, double height)
        {
            double centerX = width / 2;
            double centerY = height * 0.55; // slightly below table center
            double radius = 200; // distance from table center
            int count = Players.Count;
            double angleStep = 180.0 / (count + 1);

            for (int i = 0; i < count; i++)
            {
                // Semicircle from left to right (facing dealer at top)
                double angle = 0 + angleStep * (i + 1); // 0° = left, 180° = right
                double rad = angle * Math.PI / 180;

                // MAUI Y axis increases downward
                Players[i].X = centerX + radius * Math.Cos(rad);
                Players[i].Y = centerY + radius * Math.Sin(rad);
            }
        }
        public override void NextTurn()
        {
            Players[currentIndex].IsCurrentTurn = false;

            currentIndex = (currentIndex + 1) % Players.Count;

            Players[currentIndex].IsCurrentTurn = true;

            OnGameChanged?.Invoke(this, true);
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
                    this.Players = game.Players;
                    this.Created = game.Created;
                    this.Id = game.Id;
                    //if username not exist in list
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
                    ArrangePlayerSeats();
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
