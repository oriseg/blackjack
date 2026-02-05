
using blackjack.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace blackjack.Models
{
    public abstract class GameModel
    { 
        protected FbData fbd = new();
        public string HostName { get; set; } = string.Empty; 
        public Dealer ?Dealer { get; set; } 
        public string Id { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int PlayerCount { get; set; }
        public Dictionary<string, RoundResultData> RoundResults { get; set; } = new Dictionary<string, RoundResultData>();
    
        public ObservableCollection<Player> Players { get; set; } = [];
        protected TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        [Ignored]
        public bool countdownStarted = false;
        [Ignored]
        public DateTime GameStartTime { get;  set; }
        [Ignored]
        public int CurrentPlayerCount => Players.Count;
        [Ignored]
        public Random rnd = new();
        [Ignored]
        public ObservableCollection<PlayerCount>? PlayerCountDL { get; set; } = [new PlayerCount(2), new PlayerCount(3), new PlayerCount(4)];
        [Ignored]
        public PlayerCount SelectedPlayerCount { get; set; } = new PlayerCount();
        [Ignored]
        public EventHandler<bool>? OnGameAdded;
        [Ignored]
        public EventHandler<bool>? OnGameChanged;
        [Ignored]
        public EventHandler<bool>? OnTurnChanged;
        [Ignored]
        public EventHandler? OnPlayerTurn;
        [Ignored]
        public EventHandler? OnTimerChanged;
        [Ignored]
        public EventHandler? OnCountdownFinished;
        [Ignored]
        public EventHandler? OnGameJoined;
        [Ignored]
        public EventHandler? OnWatingMassgeChanged;
        [Ignored]
        public EventHandler? OnTimeLeftChanged;
        [Ignored]
        public EventHandler? Onbust;
        [Ignored]
        public EventHandler<RoundResultData>? OnRoundResult;
        [Ignored]
        protected IListenerRegistration? ilr;
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void ArrangePlayerSeats();
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete); 
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
        [Ignored]
        public EventHandler? OnRoundCountdownChanged;
        [Ignored]
        public EventHandler? OnRoundCountdownFinished;
        [Ignored]
        public int roundCountdown;
        [Ignored]
        public string RoundCountdownText { get;  set; } = "Next round starting in";
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


    }
}
