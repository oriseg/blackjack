using blackjack.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;

namespace blackjack.Models
{
    public abstract class GameModel
    {
        #region Fields
        protected FbData fbd = new();
        protected TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);
        [Ignored]
        public bool suppressDecisionPopup = false;
        [Ignored]
        public bool countdownStarted = false;
        [Ignored]
        public Random rnd = new();
        [Ignored]
        protected IListenerRegistration? ilr;
        #endregion

        #region Properties
        public string HostName { get; set; } = string.Empty;
        public Dealer? Dealer { get; set; }
        public string Id { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int PlayerCount { get; set; }
        public Dictionary<string, RoundResultData> RoundResults { get; set; } = new();
        public bool GameEnded { get; set; }
        public int CurrCoins { get; set; }
        public int DefaultBet { get; set; }
        [Ignored]
        public List<int> BetOptions { get; private set; } = new() { 10, 25, 50, 100, 200 };
        [Ignored]
        public int SelectedBetAmount { get; set; }
        public ObservableCollection<Player> Players { get; set; } = new();
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        [Ignored]
        public DateTime GameStartTime { get; set; }
        [Ignored]
        public int CurrentPlayerCount => Players.Count;
        [Ignored]
        public Player? CurrentLocalPlayer => Players.FirstOrDefault(p => p.UserName == Preferences.Get(Keys.NameKey, string.Empty));
        [Ignored]
        public ObservableCollection<PlayerCount>? PlayerCountDL { get; set; } = new() { new PlayerCount(2), new PlayerCount(3), new PlayerCount(4) };
        [Ignored]
        public PlayerCount SelectedPlayerCount { get; set; } = new PlayerCount();
        [Ignored]
        public string WaitingMessage
        {
            get
            {
                if (!CanStart())
                    return $"{Strings.Waitingfor} {CurrentPlayerCount}/{PlayerCount} {Strings.players}";
                if (string.IsNullOrEmpty(TimeLeft))
                    return string.Empty;
                return $"{Strings.GameStartingIn} {TimeLeft}";
            }
        }
        #endregion

        #region Events
        [Ignored] public EventHandler<bool>? OnGameAdded;
        [Ignored] public EventHandler<bool>? OnGameChanged;
        [Ignored] public EventHandler<bool>? OnTurnChanged;
        [Ignored] public EventHandler? OnPlayerTurn;
        [Ignored] public EventHandler? OnTimerChanged;
        [Ignored] public EventHandler? OnCountdownFinished;
        [Ignored] public EventHandler? OnGameJoined;
        [Ignored] public EventHandler? OnWatingMassgeChanged;
        [Ignored] public EventHandler? OnTimeLeftChanged;
        [Ignored] public EventHandler? Onbust;
        [Ignored] public EventHandler? OnGameOver;
        [Ignored] public EventHandler<RoundResultData>? OnRoundResult;
        [Ignored] public EventHandler? OnRoundCountdownChanged;
        [Ignored] public EventHandler? OnRoundCountdownFinished;
        #endregion

        #region Abstract Methods
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void ArrangePlayerSeats();
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void JoinGame(string GameCode);
        public abstract bool HostIsCurrentUser();
        public abstract Card CreateRandomCard();
        public abstract Task DealerTurn();
        public abstract void ArrangeSeats(double width, double height);
        public abstract void OnMessageReceived(long timeLeft);
        public abstract void RegisterTimer();
        public abstract void CreateGame(int playerCount);
        public abstract void UpdatePlayersTurnState();
        public abstract void OnComplete(Task task);
        public abstract void OnComplete(IQuerySnapshot qs);
        public abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        public abstract void NextTurn();
        public abstract void DealPlayersCards();
        public abstract void DealDealerCards();
        public abstract void DealCards();
        public abstract void CheckAndStartCountdown();
        public abstract void CheckLocalPlayerTurn();
        public abstract void Stand();
        public abstract void Hit();
        public abstract void Double();
        public abstract bool CanStart();
        public abstract void PlayersTurnEnds();
        public abstract bool IsMyTurn();
        public abstract void ClearAndRestart();
        public abstract void EvaluateWinners();
        public abstract void ClearRoundData();
        public abstract void LeaveGame();
        public abstract void ClearDealerData();
        public abstract void ClearRoundDataForAllPlayers();
        public abstract void HandelResults(RoundResultData data);
        #endregion
    }
}