using blackjack.Models;
using blackjack.ModelsLogic;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace blackjack.ViewModels
{
    public partial class GameTableVM : ObservableObject
    {
        private readonly Game game; 
        public ObservableCollection<Player> Players => game.Players;
        public ICommand NextTurnCommand => new Command(NextTurn);
        public bool IsMyTurn => game.IsMyTurn(); 



        public GameTableVM (Game game)
        {
            this.game = game; 
            game.OnGameAdded+= OnGameAdded;
            game.OnGameChanged+= OnGameChanged;
            game.OnTurnChanged += OnTurnChanged;
            game.AddSnapshotListener();
            game.ArrangePlayerSeats();
        }

        private void OnTurnChanged(object? sender, bool e)
        {
            UpdatePlayersTurnState();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(Players));
        }

        private void NextTurn()
        {
            game.NextTurn();
        }
        private void UpdatePlayersTurnState()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].IsCurrentTurn = (i == game.CurrentPlayerIndex);
            }
        }
        private void OnGameAdded(object? sender, bool e)
        {
            UpdatePlayersTurnState();
            OnPropertyChanged(nameof(Players));
        }
        private void OnGameChanged(object? sender, bool e)
        {
            OnPropertyChanged(nameof(Players));
        }
        

        public void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
    }
}
