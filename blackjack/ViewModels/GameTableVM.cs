using blackjack.Models;
using blackjack.ModelsLogic;
using System.Collections.ObjectModel;
namespace blackjack.ViewModels
{
    public partial class GameTableVM : ObservableObject
    {
        private readonly Game game;
        public GameTableVM (Game game)
        {
            this.game = game; 
            game.OnGameAdded+= OnGameAdded;
        } 

        private void OnGameAdded(object? sender, bool e)
        {
           OnPropertyChanged(nameof(Players));
        }
        public ObservableCollection<Player> Players => game.Players;

      

    

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
