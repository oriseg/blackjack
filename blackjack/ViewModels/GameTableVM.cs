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

      

        public GameTableVM (Game game)
        {
            this.game = game; 
            game.OnGameAdded+= OnGameAdded;
            game.OnGameChanged+= OnGameChanged;
            game.AddSnapshotListener();
            game.ArrangePlayerSeats();
        }


        private void NextTurn(object obj)
        {
      
            game.NextTurn();   // Your Game class handles turn logic
            OnPropertyChanged(nameof(Players));
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
