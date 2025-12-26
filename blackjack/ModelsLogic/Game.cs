
using blackjack.Models;
using Plugin.CloudFirestore;

namespace blackjack.ModelsLogic
{
    public class Game : GameModel
    {
        public Game()
        {
            Dealer = new Dealer();
            HostName = new User().UserName;
            Created = DateTime.Now;

        }
        public Game(int playercount)
        { 
            Dealer = new Dealer();
            HostName = new User().UserName;
            Created = DateTime.Now;
            PlayerCount = playercount;

        }
        public void CreateGame(int PlayerCount)
        {
            //clean prev game after back
            int uniqueSeed = Guid.NewGuid().GetHashCode();
            Random generator = new Random(uniqueSeed);
            this.Id = generator.Next(0, 1000000).ToString("D6");
            this.Players.Clear();

            this.PlayerCount = PlayerCount;
            Player host = new Player(HostName);
            host.IsCurrentTurn = true;
            this.Players.Add(host);
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
            if (Players.Count == 0)
                return;

            int nextPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;

            // Update the CurrentPlayerIndex field in Firestore.
            // The last parameter '_ => { }' is a lambda expression (anonymous function) 
            // that acts as a callback when the update is complete. 
            // Here it is empty because we don’t need to do anything after the update.
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(CurrentPlayerIndex), nextPlayerIndex, _ => { });

            //OnTurnChanged?.Invoke(this, true);
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
                    this.PlayerCount = game.PlayerCount; 
                    this.HostName = game.HostName;
                    this.Dealer = game.Dealer;
                    //if username not exist in list
                }
            }
            OnGameJoined?.Invoke(this, EventArgs.Empty);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {

                if (Players.Count != updatedGame.Players.Count)
                {
                    Players = updatedGame.Players;
                    IsFull = updatedGame.IsFull;
                    ArrangePlayerSeats();
                }
                for(int i =0; i < Players.Count; i++)
                {
                    Players[i].PlayerHand = updatedGame.Players[i].PlayerHand; 
                }

                if (CurrentPlayerIndex != updatedGame.CurrentPlayerIndex)
                {
                    int prevCurrnetPlayerIndex = CurrentPlayerIndex;
                    CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                    Players[CurrentPlayerIndex].IsCurrentTurn = true;
                    Players[prevCurrnetPlayerIndex].IsCurrentTurn = false;
                    CheckLocalPlayerTurn();
                }
                HostName= updatedGame.HostName; 
                if(Dealer != null&& updatedGame.Dealer!=null)
                    Dealer.DealerHand = updatedGame.Dealer.DealerHand;



                OnTurnChanged?.Invoke(this, true);
                OnGameChanged?.Invoke(this, true);


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

        public bool IsMyTurn()
        {
            string currLocalUserName = Preferences.Get(Keys.NameKey, string.Empty);
            return Players[CurrentPlayerIndex].UserName.Equals(currLocalUserName);
        }  
        public override void CheckLocalPlayerTurn()
        {
            if (IsMyTurn()&& CanStart())
            {
                OnPlayerTurn?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool HostIsCurrentUser()
        {
            string currLocalUserName = Preferences.Get(Keys.NameKey, string.Empty);
            return currLocalUserName.Equals(HostName);
        }
        private Card CreateRandomCard()
        {
            int suitIndex = rnd.Next(0, 4);
            int rankIndex = rnd.Next(0, 13);

            var suit = (CardModel.Shapes)suitIndex;
            var rank = (CardModel.Ranks)rankIndex;
            string imageName = CardModel.cardsImage[suitIndex, rankIndex];

            return new Card(suit, rank, imageName);
        }
        public override void DealCards()
        {
            if (!HostIsCurrentUser())
                return;
            DealPlayersCards();
            DealDealerCards();

            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Game.Dealer), Dealer!, _ => { });
        } 

        public override void DealPlayersCards()
        {
            if (HostIsCurrentUser())
            {
                for (int i = 0; i < 2; i++)
                {
                    foreach (Player player in Players)
                    {
                        player.PlayerHand.AddCard(CreateRandomCard());
                    }
                } 
            }
            CheckLocalPlayerTurn();
        }
        public override void DealDealerCards()
        {
            for (int i = 0; i < 2; i++)
            {
                Card card = CreateRandomCard();
                card.IsFaceDown = true;
                Dealer?.DealerHand.AddCard(card);
            }
        }
        public override void CheckAndStartCountdown()
        {
            if (CanStart()&& !countdownStarted)
            {
                GameStartTime = DateTime.UtcNow.AddSeconds(5); 
                countdownStarted = true;
                _ = CountdownLoop(); 
            }
        }
        private async Task CountdownLoop()
        {
            while (GetRemainingCountdown() > 0)
            {
                OnTimerChanged?.Invoke(this, EventArgs.Empty); 
                await Task.Delay(100); 
            }  
            OnTimerChanged?.Invoke(this, EventArgs.Empty); 
            OnCountdownFinished?.Invoke(this, EventArgs.Empty);
        }

        // Get seconds remaining until countdown reaches 0
        public int GetRemainingCountdown()
        {
            int remaining = (int)(GameStartTime - DateTime.UtcNow).TotalSeconds;
            return Math.Max(0, remaining);
        } 


        public bool CanStart()
        {
           return CurrentPlayerCount >= PlayerCount;
        }

        public override void Double()
        {
            if (!IsMyTurn())
                return;
            Player current = Players[CurrentPlayerIndex];
            current.PlayerHand.AddCard(CreateRandomCard());
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });
            NextTurn();
        }
        public override void Stand()
        {
            if (!IsMyTurn())
                return;

            NextTurn();
        }
       

        public override void Hit()
        {
            if (!IsMyTurn())
                return;

            Player current = Players[CurrentPlayerIndex];
            current.PlayerHand.AddCard(CreateRandomCard());
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });

            if (current.PlayerHand.Isbust)
            {
                NextTurn();
            }
        }


    }
}
