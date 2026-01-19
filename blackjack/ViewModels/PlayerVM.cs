using blackjack.Models;
using blackjack.ModelsLogic;

namespace blackjack.ViewModels
{
   public class PlayerVM : ObservableObject
    {
        private readonly Player _player;

        public PlayerVM(Player player)
        {
            _player = player;
            PlayerHand = new HandVM(player.PlayerHand);
        }

        public string UserName => _player.UserName;

        public HandVM PlayerHand { get; }

        public bool IsCurrentTurn
        {
            get => _player.IsCurrentTurn;
            set
            {
                if (_player.IsCurrentTurn != value)
                {
                    _player.IsCurrentTurn = value;
                    OnPropertyChanged();
                }
            }
        }
        public double X => _player.X;
        public double Y => _player.Y;
    } 

}
