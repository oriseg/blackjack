using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace blackjack.ViewModels
{
    public partial class GameTableVM : ObservableObject
    {
        private readonly Game game;
        public IEnumerable<PlayerVM> Players => game.Players.Select(p => new PlayerVM(p));
        public ObservableCollection<Card> DealerCards => game.Dealer!.DealerHand.Cards;
        private Player CurrentPlayer => game.Players[game.CurrentPlayerIndex];
        public string Id => game.Id;
        public int SelectedPlayerCount => game.PlayerCount;
        public int CurrentPlayerCount => game.CurrentPlayerCount;
        public string TimeLeft => game.TimeLeft;
        public string WaitingMessage=> game.WaitingMessage;
        public bool CanStart => game.CanStart();
        public bool IsMyTurn => game.IsMyTurn();
        public int CurrentHandValue => CurrentPlayer.PlayerHand.HandValue;
        public Color CurrentHandColor => CurrentPlayer.PlayerHand.HandColor;
        public bool CurrentHandIsBust => CurrentPlayer.PlayerHand.IsBust;
        public int CurrentDealerHandValue=> game.Dealer!.DealerHand.HandValue;
        public GameTableVM(Game game)
        {
            this.game = game;
                // Subscribe to game events
            game.OnGameAdded += OnGameAdded;
            game.OnGameChanged += OnGameChanged;
            game.OnTurnChanged += OnTurnChanged;
            game.OnTimerChanged += OnTimerChanged;
            game.OnCountdownFinished += OnCountdownFinished;
            game.OnPlayerTurn += OnPlayerTurn;
            game.OnWatingMassgeChanged += OnWatingMassgeChanged;
            game.Dealer!.DealerHand.OnHandValueChanged += DealerHandValueChanged;
            // Subscribe current player hand 

            CurrentPlayer.PlayerHand.OnHandValueChanged += HandValueChanged;
            CurrentPlayer.PlayerHand.OnHandColorChanged += HandColorChanged;
            CurrentPlayer.PlayerHand.OnHandStateChanged += HandStateChanged; 
            game.AddSnapshotListener();
            game.ArrangePlayerSeats();
        }

        private void DealerHandValueChanged(object? sender, EventArgs e)
        {
          OnPropertyChanged(nameof(CurrentDealerHandValue));
        }

        private void HandValueChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentHandValue));
        }

        private void HandColorChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentHandColor));
        }

        private void HandStateChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentHandIsBust));
        }

        private void OnWatingMassgeChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(WaitingMessage));
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
            game.UpdatePlayersTurnState();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(Players));
        }




        private void OnGameAdded(object? sender, bool e)
        {
            game.UpdatePlayersTurnState();
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
            OnPropertyChanged(nameof(CurrentDealerHandValue));
            game.CheckAndStartCountdown();
        }

        public void AddSnapshotListener() => game.AddSnapshotListener();
        public void RemoveSnapshotListener() => game.RemoveSnapshotListener();


    }
}
