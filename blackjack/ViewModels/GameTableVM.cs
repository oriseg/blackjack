using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public partial class GameTableVM : ObservableObject
    {
        private readonly Game game;
        public ObservableCollection<Player> Players => game.Players;
        public ObservableCollection<Card> DealerCards => game.Dealer!.DealerHand.Cards;
        public string Id => game.Id;
        public int SelectedPlayerCount => game.PlayerCount;
        public int CurrentPlayerCount => Players.Count;
        public string WaitingMessage
        {
            get
            {
                if (!CanStart)
                    return $"{Strings.Waitingfor} {CurrentPlayerCount}/{SelectedPlayerCount} {Strings.players}";

                return $"Game starting in {game.GetRemainingCountdown()}";
            }
        }

        public bool CanStart => game.CanStart();
        public bool IsMyTurn => game.IsMyTurn();


        public GameTableVM(Game game)
        {
            this.game = game;
            game.OnGameAdded += OnGameAdded;
            game.OnGameChanged += OnGameChanged;
            game.OnTurnChanged += OnTurnChanged;
            game.OnTimerChanged += OnTimerChanged;
            game.OnCountdownFinished += OnCountdownFinished;
            game.OnPlayerTurn += OnPlayerTurn;
            game.AddSnapshotListener();
            game.ArrangePlayerSeats();
        }

        private async void OnPlayerTurn(object? sender, EventArgs e)
        {
          await Application.Current!.MainPage!.ShowPopupAsync(new DecisionPopUp(game));
        }

        private void OnCountdownFinished(object? sender, EventArgs e)
        {
            game.DealCards();
        }

        private void OnTimerChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(WaitingMessage));
        }

        private void OnTurnChanged(object? sender, bool e)
        {
            UpdatePlayersTurnState();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(Players));
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
            OnPropertyChanged(nameof(SelectedPlayerCount));
            OnPropertyChanged(nameof(WaitingMessage)); 
            OnPropertyChanged(nameof(DealerCards));
        }

        private void OnGameChanged(object? sender, bool e)
        {
            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(SelectedPlayerCount));
            OnPropertyChanged(nameof(WaitingMessage));
            OnPropertyChanged(nameof(DealerCards));
            game.CheckAndStartCountdown();
        }

        public void AddSnapshotListener() => game.AddSnapshotListener();
        public void RemoveSnapshotListener() => game.RemoveSnapshotListener();


    }
}
