using blackjack.Models;
using blackjack.ModelsLogic;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public partial class LogInPageVM : ObservableObject
    {
        #region Fields
        private readonly User user = new();
        #endregion

        #region Commands
        public ICommand LogInCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        #endregion

        #region Properties
        public bool IsPassword { get; set; } = true;

        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LogInCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (LogInCommand as Command)?.ChangeCanExecute();
            }
        }
        #endregion

        #region Constructor
        public LogInPageVM()
        {
            LogInCommand = new Command(LogIn, CanLogIn);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
        }
        #endregion

        #region Public Methods
        public bool CanLogIn()
        {
            return !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }
        #endregion

        #region Private Methods
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            }
        }

        private void LogIn()
        {
            user.Login();
        }
        #endregion
    }
}