using blackjack.Models;
using blackjack.ModelsLogic;
using System.Collections.ObjectModel;
namespace blackjack.ViewModels
{
    public partial class GameTableVM : ObservableObject
    {
        private readonly Game game; 
        private readonly SeatsArrangement seatsArrangement = new();
        public ObservableCollection<Player> Players => game.Players;

        public GameTableVM (Game game)
        {
            this.game = game; 
            game.OnGameAdded+= OnGameAdded;
            game.OnGameChanged+= OnGameChanged;
            game.AddSnapshotListener();
            game.ArrangePlayerSeats();
        }

 

        private void OnGameAdded(object? sender, bool e)
        {
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
