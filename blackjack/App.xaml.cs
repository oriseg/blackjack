using blackjack.ModelsLogic;
using blackjack.Views;

namespace blackjack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
            //User user = new();
            //Page page = user.IsRegistered ? new LoginPage() : new RegisterPage();
            //MainPage = page;
        }
    }
}
