using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private readonly Game game;

        public ICommand JoinGameCommand => new Command(JoinGame);
        public ICommand CreateGameCommand => new Command(CreateGame);

        // Expose Game Properties (NO LOGIC)
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

        public MainPageVM()
        {
            game = new Game();

            game.OnGameJoined += OnGameJoined;
            game.OnGameAdded += OnGameAdded;
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
    }
}
