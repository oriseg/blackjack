using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace blackjack.Models
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        #region Public Methods
        protected void OnPropertyChanged([CallerMemberName] string? propertyChanged = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }
        #endregion

    }
}