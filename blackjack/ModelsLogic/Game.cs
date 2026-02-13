
using blackjack.Models;
using CommunityToolkit.Mvvm.Messaging;
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
            RegisterTimer();

        }
        public Game(int playercount)
        {
            Dealer = new Dealer();
            HostName = new User().UserName;
            Created = DateTime.Now;
            PlayerCount = playercount;
            RegisterTimer();

        }
        // עדכון מי בתור
        public void UpdatePlayersTurnState()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].IsCurrentTurn = (i == CurrentPlayerIndex);
            }
        }
        public void CreateGame(int PlayerCount)
        {
            //clean prev game after back
            int uniqueSeed = Guid.NewGuid().GetHashCode();
            Random generator = new(uniqueSeed);
            this.Id = generator.Next(0, Keys.IdGenerator).ToString("D6");
            this.Players.Clear();
            this.PlayerCount = PlayerCount;
            Player host = new(HostName);
            host.IsCurrentTurn = true;
            this.Players.Add(host);
            this.SetDocument(OnComplete);

        }
        private void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
                if (m.Value == Keys.FinishedSignal)
                {
                    OnCountdownFinished?.Invoke(this, EventArgs.Empty);
                }
            });
        }
        private void OnMessageReceived(long timeLeft)
        {
            TimeLeft = timeLeft == Keys.FinishedSignal ? String.Empty : double.Round(timeLeft / 1000, 1).ToString();
            OnTimeLeftChanged?.Invoke(this, EventArgs.Empty);
            OnWatingMassgeChanged?.Invoke(this, EventArgs.Empty);
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
            if (CurrentPlayerIndex >= Players.Count - 1)
            {
                PlayersTurnEnds();
                return;
            }
            int nextPlayerIndex = CurrentPlayerIndex + 1;
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
        public void JoinGame(string GameCode)
        {
            Player joinedPlayer = new(HostName);
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

                for (int i = 0; i < Players.Count; i++)
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

                HostName = updatedGame.HostName;

                if (Dealer != null && updatedGame.Dealer != null)
                    Dealer.DealerHand = updatedGame.Dealer.DealerHand;

                string myUserName = Preferences.Get(Keys.NameKey, string.Empty);

                if (updatedGame.RoundResults != null && updatedGame.RoundResults.TryGetValue(myUserName, out RoundResultData? myResult))
                {
                    OnRoundResult?.Invoke(this, myResult);
                }

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

        public override bool IsMyTurn()
        {
            string currLocalUserName = Preferences.Get(Keys.NameKey, string.Empty);
            return Players[CurrentPlayerIndex].UserName.Equals(currLocalUserName);
        }
        public override void CheckLocalPlayerTurn()
        {
            if (!suppressDecisionPopup && IsMyTurn() && CanStart())
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
                Card card = CreateRandomCard();
                Dealer?.DealerHand.AddCard(card);        
        }
        public override void CheckAndStartCountdown()
        {
            if (CanStart() && !countdownStarted)
            {
                countdownStarted = true;
                OnWatingMassgeChanged?.Invoke(this, EventArgs.Empty);
                WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(timerSettings));
            }
        }

        public override bool CanStart()
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
            if (current.PlayerHand.IsBust)
            {
                Onbust?.Invoke(this, EventArgs.Empty); 
                NextTurn();
            }
        }
        public override async void PlayersTurnEnds()
        {

            Player current = Players[CurrentPlayerIndex];
            current.IsCurrentTurn = false;
            bool allPlayersDone = Players.All(p => p.PlayerHand.IsBust || !p.IsCurrentTurn);
            if (allPlayersDone)
               await DealerTurn();
            else
                NextTurn();
            // Update the database after turn ends
            if (HostIsCurrentUser())
            {
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Game.Dealer), Dealer!, _ => { });
            }
        }
        private async Task DealerTurn()
        {
            if (Dealer == null) return;
            // Dealer keeps drawing cards until 17 or more
            while (Dealer.DealerHand.HandValue < 17)
            {
                await Task.Delay(Keys.TwoSecondDelay);
                Dealer.DealerHand.AddCard(CreateRandomCard());
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Game.Dealer), Dealer!, _ => { });
            }
            EvaluateWinners();          
        }

         public override void EvaluateWinners()
        {
            Dictionary<string, RoundResultData> results = [];

            foreach (Player player in Players)
            {
                RoundResultData result = new()
                {
                    TargetUserName = player.UserName
                };

                if (player.PlayerHand.IsBust)
                {
                    result.Title = "💥 " + Strings.Bust;
                    result.Message = Strings.WentOver21;
                }
                else if (Dealer!.DealerHand.IsBust)
                {
                    result.Title = "🎉 " + Strings.YouWin;
                    result.Message = Strings.Dealerbusted;
                }
                else if (player.PlayerHand.HandValue > Dealer.DealerHand.HandValue)
                {
                    result.Title = "🏆 " + Strings.YouWin;
                    result.Message = Strings.GreatHand;
                }
                else if (player.PlayerHand.HandValue < Dealer.DealerHand.HandValue)
                {
                    result.Title = "😞 " + Strings.Lost;
                    result.Message = Strings.DealerWins;
                }
                else
                {
                    result.Title = "🤝 " + Strings.Push;
                    result.Message = Strings.tie;
                }
                results[player.UserName] = result;
            }

            // SAVE RESULTS TO FIRESTORE
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(RoundResults), results, _ => { });
        }
        public override void ClearRoundData()
        {
            if (!HostIsCurrentUser())
                return;

            suppressDecisionPopup = true; // prevent premature turn popups

            // Reset RoundResults in Firestore
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(RoundResults), new Dictionary<string, RoundResultData>(), _ => { });

            // Reset players
            foreach (var player in Players)
            {
                // Ensure PlayerHand is not null
                if (player.PlayerHand == null)
                    player.PlayerHand = new Hand();
                else
                    player.PlayerHand.Clear();

                player.PlayerHand.HandValue = 0;
                player.PlayerHand.IsBust = false;
                player.PlayerHand.HandColor = Colors.Black;

                player.IsCurrentTurn = false; // clear current turn
            }

            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });

            // Reset dealer
            if (Dealer != null)
            {
                if (Dealer.DealerHand == null)
                    Dealer.DealerHand = new Hand();
                else
                    Dealer.DealerHand.Clear();

                Dealer.DealerHand.HandValue = 0;
            }

            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Dealer), Dealer!, _ => { });

            // Reset current player index
            CurrentPlayerIndex = 0;
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(CurrentPlayerIndex), CurrentPlayerIndex, _ => { });
        }

        public override void ClearAndRestart()
        {
            // Step 1: clear previous round safely
            ClearRoundData();

            // Step 2: set first player as current turn
            if (Players.Count > 0)
            {
                Players[0].IsCurrentTurn = true;
                CurrentPlayerIndex = 0;
            }

            // Step 3: deal new round cards
            DealCards();

            // Step 4: allow decision popups again
            suppressDecisionPopup = false;

            //// Step 5: notify local player if it's their turn
            //CheckLocalPlayerTurn();
        }

    }

}



       



