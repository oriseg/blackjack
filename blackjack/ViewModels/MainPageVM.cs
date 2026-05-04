using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        #region Fields
        private readonly Game game;
    
        #endregion

        #region Commands
        public ICommand JoinGameCommand => new Command(JoinGame);
        public ICommand CreateGameCommand => new Command(CreateGame);
        public ICommand ProfileCommand { get; private set; }
        #endregion

        #region Properties 
        public ObservableCollection<PlayerCount>? PlayerCount => game.PlayerCountDL;

        public PlayerCount SelectedPlayerCount
        {
            get => game.SelectedPlayerCount;
            set => game.SelectedPlayerCount = value;
        }

        public List<int> BetOptions => game.BetOptions;

        public int SelectedBetAmount
        {
            get => game.SelectedBetAmount;
            set => game.SelectedBetAmount = value;
        }

        public string? GameCode { get; set; }
        #endregion

        #region Constructor
        public MainPageVM()
        {
            game = new Game();
            game.OnGameJoined += OnGameJoined;
            game.OnGameAdded += OnGameAdded;
            ProfileCommand = new Command(Open);
        }
        #endregion


        #region Private Methods 
        private void Open(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new ProfilePage());
            });
        }
        private void CreateGame(object obj)
        {
            game.CreateGame(SelectedPlayerCount.Count);

            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GameTable(game));
            });
        }

        private void JoinGame()
        {
            if (!string.IsNullOrEmpty(GameCode))
                game.JoinGame(GameCode);
        }

        private void OnGameJoined(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GameTable(game));
            });
        }

        private void OnGameAdded(object? sender, bool e)
        {
            OnPropertyChanged(nameof(PlayerCount));
        }
        #endregion
    }
}