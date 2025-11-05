
using blackjack.ModelsLogic;

namespace blackjack.Models
{
    internal abstract class GameModel
    { 
        protected FbData fbd = new();
        public string HostName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public EventHandler<bool>? OnGameAdded;
    }
}
