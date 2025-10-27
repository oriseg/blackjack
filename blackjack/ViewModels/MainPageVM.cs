using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    internal class MainPageVM : ObservableObject
    {
        private readonly User user = new();
        public ICommand ShowJoinPopupCommand { get; private set; }

        public MainPageVM()
        {
            ShowJoinPopupCommand = new Command(ShowJoinPopup);
        }

        private void ShowJoinPopup(object obj)
        {
            Shell.Current.ShowPopup(new JoinPopup());
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

