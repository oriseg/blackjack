using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    internal class MainPageVM : ObservableObject
    {
        private readonly Game game = new();
        public ICommand ShowJoinPopupCommand => new Command(ShowJoinPopup); 
        public ICommand CreateGameCommand => new Command(CreateGame);
        public ObservableCollection<PlayerCount>? PlayerCount { get => game.PlayerCount; set => game.PlayerCount = value; }
        public PlayerCount SelectedPlayerCount { get => game.SelectedPlayerCount; set => game.SelectedPlayerCount = value; }
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

