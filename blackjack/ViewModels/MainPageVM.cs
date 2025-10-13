using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    internal partial class MainPageVM : ObservableObject
    {
        public ICommand NavToLoginCommand { get => new Command(NavToLogin); }
         
        public ICommand NavToRegisterCommand { get => new Command(NavToRegister); }

        private async void NavToRegister()
        {
           await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        private async void NavToLogin()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
