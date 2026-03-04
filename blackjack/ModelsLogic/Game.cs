

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
        public override void UpdatePlayersTurnState()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].IsCurrentTurn = (i == CurrentPlayerIndex);
            }
        }
        public override void CreateGame(int playerCount)
        {
            DefaultBet = SelectedBetAmount;
            GetCurrentUserCoins();
            int uniqueSeed = Guid.NewGuid().GetHashCode();
            Random generator = new(uniqueSeed);
            Id = generator.Next(0, Keys.IdGenerator).ToString("D6");

            Players.Clear();
            PlayerCount = playerCount;

            Player host = new(HostName);
            host.IsCurrentTurn = true;

            Players.Add(host);
            SetDocument(OnComplete);
        }
        public override void RegisterTimer()
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
        public override void OnMessageReceived(long timeLeft)
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
        public override void ArrangeSeats(double width, double height)
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
            if (Players.Count > 0)
            {
                if (CurrentPlayerIndex >= Players.Count - 1)
                {
                    PlayersTurnEnds();
                }
                else
                {
                    int nextPlayerIndex = CurrentPlayerIndex + 1;
                    fbd.UpdateFields(Keys.GamesCollection,Id,nameof(CurrentPlayerIndex),nextPlayerIndex,_ => { });
                }
            }
        }
        public override void OnComplete(Task task)
        {
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
        }
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void JoinGame(string GameCode)
        {
            fbd.CheckGameCode(GameCode, (isValid) =>
            {
                if (isValid)
                {
                    Player joinedPlayer = new(HostName);
                    fbd.UpdateFields(Keys.GamesCollection, GameCode,"Players",FieldValue.ArrayUnion(joinedPlayer), OnComplete);
                    GetCurrentUserCoins();
                }
            });
        }
        public override void OnComplete(IQuerySnapshot qs)
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
                    this.DefaultBet = game.DefaultBet;
                }
            }
            OnGameJoined?.Invoke(this, EventArgs.Empty);
        }

        public override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                if (updatedGame.GameEnded)
                {
                    OnGameOver?.Invoke(this, EventArgs.Empty);

                    ilr?.Remove();        // stop listening
                   // DeleteDocument(_ => { }); // delete safely

                    return;
                }
                // 1️⃣ Update players count
                if (Players.Count != updatedGame.Players.Count)
                {
                    Players = updatedGame.Players;
                    IsFull = updatedGame.IsFull;
                    ArrangePlayerSeats();
                }

                // 2️⃣ Update players' hands
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].PlayerHand = updatedGame.Players[i].PlayerHand;
                }

                // 3️⃣ Update current turn
                if (CurrentPlayerIndex != updatedGame.CurrentPlayerIndex)
                {
                    int prevCurrnetPlayerIndex = CurrentPlayerIndex;
                    CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                    Players[CurrentPlayerIndex].IsCurrentTurn = true;
                    Players[prevCurrnetPlayerIndex].IsCurrentTurn = false;
                    CheckLocalPlayerTurn();
                }

                // 4️⃣ Update dealer
                HostName = updatedGame.HostName;
                if (Dealer != null && updatedGame.Dealer != null)
                    Dealer.DealerHand = updatedGame.Dealer.DealerHand;

                // 5️⃣ Update round results for local player
                string myUserName = Preferences.Get(Keys.NameKey, string.Empty);
                if (updatedGame.RoundResults != null &&
                    updatedGame.RoundResults.TryGetValue(myUserName, out RoundResultData? myResult))
                {
                    OnRoundResult?.Invoke(this, myResult);
                }

                // ✅ 6️⃣ **Update UsersMap coins**
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
            if (HostIsCurrentUser())
            {
                ilr?.Remove();
                DeleteDocument(OnComplete);
            }

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

        public override bool HostIsCurrentUser()
        {
            string currLocalUserName = Preferences.Get(Keys.NameKey, string.Empty);
            return currLocalUserName.Equals(HostName);
        }
        public override Card CreateRandomCard()
        {
            int suitIndex = rnd.Next(0, 4);
            int rankIndex = rnd.Next(0, 13);
            CardModel.Shapes suit = (CardModel.Shapes)suitIndex;
            CardModel.Ranks rank = (CardModel.Ranks)rankIndex;
            string imageName = CardModel.cardsImage[suitIndex, rankIndex];
            return new Card(suit, rank, imageName);
        }
        public override void DealCards()
        {
            if (HostIsCurrentUser())
            {
                DealPlayersCards();
                DealDealerCards();
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Game.Dealer), Dealer!, _ => { });
            }
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
            if (IsMyTurn())
            {
                Player current = Players[CurrentPlayerIndex];
                current.PlayerHand.AddCard(CreateRandomCard());
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });
                NextTurn();
            }
        }
        public override void Stand()
        {
            if (IsMyTurn())
                   NextTurn();

        }
        public override void Hit()
        {
            if (IsMyTurn())
            {
                Player current = Players[CurrentPlayerIndex];
                current.PlayerHand.AddCard(CreateRandomCard());
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });
                if (current.PlayerHand.IsBust)
                {
                    Onbust?.Invoke(this, EventArgs.Empty);
                    NextTurn();
                }
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
        public override async Task DealerTurn()
        {
            if (Dealer != null)
            {
                while (Dealer.DealerHand.HandValue < 17)
                {
                    await Task.Delay(Keys.TwoSecondDelay);
                    Dealer.DealerHand.AddCard(CreateRandomCard());
                    fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Game.Dealer), Dealer!, _ => { });
                }
                EvaluateWinners();
            }    
        }

        public void GetCurrentUserCoins()
        {
            string userName = Preferences.Get(Keys.NameKey, string.Empty);
             GetCoinsForPlayer(userName);
        }
        public void GetCoinsForPlayer(string userName)
        {
            fbd.GetDocument("Users", userName, OnUserLoaded);
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
                CurrCoins = userFromDb!.Coins;
            }


        }
        public void UpdateCoinsForPlayer(string userName, int coinChange)
        {             
            fbd.GetDocument("Users", userName, (snapshot, error) =>
            {
                if (error == null && snapshot != null && snapshot.Exists)
                {
                    User? user = snapshot.ToObject<User>();
                    if (user != null)
                    {
                        int newCoins = user.Coins + coinChange;
                        newCoins = Math.Max(newCoins, 0); // prevent negative coins
                        CurrCoins = newCoins;
                        fbd.UpdateFields("Users", userName, nameof(User.Coins), newCoins, _ => { });
                    }
                }
            });
        }
        public override void EvaluateWinners()
        {
            string currLocalUserName = Preferences.Get(Keys.NameKey, string.Empty);
            Dictionary<string, RoundResultData> results = new();

            foreach (Player player in Players)
            {
                    RoundResultData result = new()
                    {
                        TargetUserName = player.UserName
                    };

                    // Determine round result
                    if (player.PlayerHand.IsBust)
                    {
                        result.Title = "💥 " + Strings.Bust;
                        result.Message = Strings.WentOver21; 
                        result.Outcome = RoundOutcome.Lose;
                    }
                    else if (Dealer!.DealerHand.IsBust)
                    {
                        result.Title = "🎉 " + Strings.YouWin;
                        result.Message = Strings.Dealerbusted; 
                        result.Outcome = RoundOutcome.Win;
                    }
                    else if (player.PlayerHand.HandValue > Dealer.DealerHand.HandValue)
                    {
                        result.Title = "🏆 " + Strings.YouWin;
                        result.Message = Strings.GreatHand; 
                        result.Outcome = RoundOutcome.Win;
                    }
                    else if (player.PlayerHand.HandValue < Dealer.DealerHand.HandValue)
                    {
                        result.Title = "😞 " + Strings.Lost;
                        result.Message = Strings.DealerWins; 
                        result.Outcome = RoundOutcome.Lose;
                    }
                    else
                    {
                        result.Title = "🤝 " + Strings.Push;
                        result.Message = Strings.tie; 
                        result.Outcome = RoundOutcome.Push;
                    }

                    results[player.UserName] = result;
                
            }

            // SAVE RESULTS TO FIRESTORE
            fbd.UpdateFields(Keys.GamesCollection, Id, nameof(RoundResults), results, _ => { });


            OnGameChanged?.Invoke(this, true);
        }


        public void HandelResults(RoundResultData data)
        {
            switch (data.Outcome)
            {
                case RoundOutcome.Win:
                    UpdateCoinsForPlayer(data.TargetUserName, DefaultBet);
                    break; 
                case RoundOutcome.Lose:
                    UpdateCoinsForPlayer(data.TargetUserName, -DefaultBet);
                    break; 
                case RoundOutcome.Push:
                    // No coin change for a push
                    break;
            }
        }
        public void ClearRoundDataForAllPlayers()
        {
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
        }
        public void ClearDealerData()
        {
            if (Dealer != null)
            {
                if (Dealer.DealerHand == null)
                    Dealer.DealerHand = new Hand();
                else
                    Dealer.DealerHand.Clear();

                Dealer.DealerHand.HandValue = 0;
            }
        }
        public override void ClearRoundData()
        {
            if (HostIsCurrentUser())
            {
                suppressDecisionPopup = true; // prevent premature turn popups
                // Reset RoundResults in Firestore
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(RoundResults), new Dictionary<string, RoundResultData>(), _ => { });
                // Reset players
                ClearRoundDataForAllPlayers();

                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Players), Players, _ => { });

                // Reset dealer
                ClearDealerData();

                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(Dealer), Dealer!, _ => { });

                // Reset current player index
                CurrentPlayerIndex = 0;
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(CurrentPlayerIndex), CurrentPlayerIndex, _ => { });
            }    
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
        }
        public void LeaveGame()
        {
            if (HostIsCurrentUser())
            {
                // 1️⃣ mark game as ended
                fbd.UpdateFields(Keys.GamesCollection, Id, nameof(GameEnded), true, _ => { });
            }
            ClearRoundDataForAllPlayers();
            ClearDealerData();
            CurrentPlayerIndex = 0;
        }

    }

}



       



