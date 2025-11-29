using Plugin.CloudFirestore.Attributes;
using System.ComponentModel;
namespace blackjack.Models
{
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string UserName { get; set; } = string.Empty;
        
        private bool _isCurrentTurn;
        [Ignored]
        public bool IsCurrentTurn 
        { 
            get => _isCurrentTurn; 
            set
            {
                _isCurrentTurn = value;
                OnPropertyChanged(nameof(IsCurrentTurn));
            }
        } 
        [Ignored]
        public double X { get; set; }
        [Ignored]
        public double Y { get; set; }
        public Player(string username)
        {
            UserName= username;
        }
        public Player()
        {

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
