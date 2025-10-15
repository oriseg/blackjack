using blackjack.Models;
using blackjack.ModelsLogic;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    internal class HomePageVM : ObservableObject
    {
        private readonly User user = new();
        public HomePageVM()
        {
       
        }
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
               
            }
        }
    }
}
