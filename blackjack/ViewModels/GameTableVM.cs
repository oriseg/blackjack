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
        public bool IsMyTurn => game.IsMyTurn();
        public string Id => game.Id;
        public int SelectedPlayerCount => game.PlayerCount;
        public int CurrentPlayerCount => Players.Count;
        public string WaitingMessage => $"{Strings.Waitingfor} {CurrentPlayerCount}/{SelectedPlayerCount} {Strings.players}";
        public bool CanStart => game.CanStart();
        public ICommand NextTurnCommand => new Command(NextTurn,CanNextTurn);
        public ICommand DealCardsCommand => new Command(DealCards, CanDealCards);

      

        public GameTableVM (Game game)
        {
            this.game = game; 
            game.OnGameAdded+= OnGameAdded;
            game.OnGameChanged+= OnGameChanged;
            game.OnTurnChanged += OnTurnChanged;
            game.AddSnapshotListener();
            game.ArrangePlayerSeats();
        }

        private void OnTurnChanged(object? sender, bool e)
        {
            UpdatePlayersTurnState();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(Players));
        }
        private void DealCards()
        {
            game.DealCards();
        }
        private void NextTurn()
        {
            game.NextTurn();
        } 
        private bool CanNextTurn()
        {
            return IsMyTurn&&CanStart;
        } 
        private bool CanDealCards()
        {
            return CanStart;
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

        }
        private void OnGameChanged(object? sender, bool e)
        {
            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(SelectedPlayerCount));
            OnPropertyChanged(nameof(WaitingMessage));


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
