using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    internal class MainPageVM : ObservableObject
    {
        private readonly Game game = new();
        public ICommand ShowJoinPopupCommand => new Command(ShowJoinPopup);
        public ICommand CreateGameCommand => new Command(CreateGame);

        public MainPageVM()
        {
            game.OnGameAdded += OnGameAdded;

        }
        private void CreateGame(object obj)
        {
            game.crateGame();
        }

        private void OnGameAdded(object? sender, bool e)
        {
          OnPropertyChanged(nameof(e));
        }

      

        private void ShowJoinPopup(object obj)
        {
            Shell.Current.ShowPopup(new JoinPopup());
        }

      
    }

}

