using blackjack.Models;
using blackjack.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public partial class HomePageVM : ObservableObject
    {

        #region Commands
        public ICommand NavToLoginCommand => new Command(NavToLogin);

        public ICommand NavToRegisterCommand => new Command(NavToRegister);
        #endregion


        #region Private Methods
        private void NavToRegister()
        {
            if (Application.Current != null)
                Application.Current.MainPage = new RegisterPage();
        }

        private void NavToLogin()
        {
            if (Application.Current != null)
                Application.Current.MainPage = new LoginPage();
        }
        #endregion
    }
}