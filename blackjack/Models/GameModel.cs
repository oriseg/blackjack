
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
        [Ignored]
        public ObservableCollection<PlayerCount>? PlayerCountDL { get; set; } = [new PlayerCount(2), new PlayerCount(3), new PlayerCount(4)];
        public int PlayerCount { get; set; }
        [Ignored]
        public PlayerCount SelectedPlayerCount { get; set; } = new PlayerCount();
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        [Ignored]
        public EventHandler<bool>? OnGameAdded;
        [Ignored]
        public EventHandler<bool>? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameJoined;
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        [Ignored]
        protected IListenerRegistration? ilr;
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
    }
}
