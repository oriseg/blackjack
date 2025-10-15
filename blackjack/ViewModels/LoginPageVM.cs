using Microsoft.Extensions.Logging;
using System.Windows.Input;
using blackjack.Models;
using blackjack.ModelsLogic;

namespace blackjack.ViewModels
{
    internal class LogInPageVM : ObservableObject
    {
        private readonly User user = new();
        public ICommand LogInCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public bool IsPassword { get; set; } = true;
        public LogInPageVM()
        {
            LogInCommand = new Command(LogIn, CanLogIn);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
        }

        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        public bool CanLogIn()
        {
            return  !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }

        private void LogIn()
        {
            user.Login();
        }

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
    }

}
