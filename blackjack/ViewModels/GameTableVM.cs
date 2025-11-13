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
        }
        public ObservableCollection<Player> Players => game.Players;

        private void OnGameChanged(object? sender, EventArgs e)
        {
          
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
