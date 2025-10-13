using Microsoft.Extensions.Logging;
using System.Windows.Input;
using blackjack.Models;
using blackjack.ModelsLogic;

namespace blackjack.ViewModels
{
    internal class LogInPageVM
    {
        private readonly User user = new();
        public ICommand LogInCommand { get; }
        public LogInPageVM()
        {
            LogInCommand = new Command(LogIn, CanLogIn);
        }
        public bool CanLogIn()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }

        private void LogIn()
        {
            user.Login();
        }

        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LogInCommand as Command)?.ChangeCanExecute();
            }
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
