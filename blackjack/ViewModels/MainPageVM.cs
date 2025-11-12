using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using CommunityToolkit.Maui.Views;
using Firebase.Auth;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    internal class MainPageVM : ObservableObject
    {
        private readonly Game game = new();
        public ICommand JoinGameCommand => new Command(JoinGame); 
        public ICommand CreateGameCommand => new Command(CreateGame);
        public ObservableCollection<PlayerCount>? PlayerCount { get => game.PlayerCountDL; set => game.PlayerCountDL = value; }
        public PlayerCount SelectedPlayerCount { get => game.SelectedPlayerCount; set => game.SelectedPlayerCount = value; }
        public string? GameCode { get; set; }
       
        public MainPageVM()
        {
            game.OnGameAdded += OnGameAdded;

        }
        private void CreateGame(object obj)
        {
            game.createGame(SelectedPlayerCount.Count); 
           
        }

        private void OnGameAdded(object? sender, bool e)
        {
          OnPropertyChanged(nameof(e));
        }

        private void JoinGame()
        {
            if (GameCode != null)
            {
                game.joinGame(GameCode);
            }
            
        } 


    }

}

