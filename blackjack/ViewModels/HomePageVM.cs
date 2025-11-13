using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public partial class HomePageVM : ObservableObject
    {
        public ICommand NavToLoginCommand => new Command(NavToLogin);

        public ICommand NavToRegisterCommand => new Command(NavToRegister); 

        private  void NavToRegister()
        {
            if (Application.Current != null)
                Application.Current.MainPage = new RegisterPage();
        }

        private  void NavToLogin()
        {
            if (Application.Current != null)
                Application.Current.MainPage = new LoginPage();
        }
    }
}
