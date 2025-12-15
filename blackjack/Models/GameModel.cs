
using blackjack.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;

namespace blackjack.Models
{
    public abstract class GameModel
    { 
        protected FbData fbd = new();
        public string HostName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public int PlayerCount { get; set; }  
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
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
        public EventHandler? OnTimerChanged;
        [Ignored]
        public EventHandler? OnCountdownFinished;
        [Ignored]
        public EventHandler? OnGameJoined;
        [Ignored] 
        protected IListenerRegistration? ilr;
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void ArrangePlayerSeats();
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete); 
        public abstract void NextTurn();
        public abstract void DealCards();
        public abstract void CheckAndStartCountdown();

    }
}
