
using blackjack.ModelsLogic;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace blackjack.Models
{
    internal abstract class GameModel
    { 
        protected FbData fbd = new();
        public string HostName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public ObservableCollection<PlayerCount>? PlayerCount { get; set; } = [new PlayerCount(2), new PlayerCount(3), new PlayerCount(4)];
        public PlayerCount SelectedPlayerCount { get; set; } = new PlayerCount();
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public EventHandler<bool>? OnGameAdded;
    }
}
