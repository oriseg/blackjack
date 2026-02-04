
using blackjack.Models;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public class ResultPopupVM
    {
            public string Title { get; }
            public string Message { get; }
            public ICommand CloseCommand { get; }

            public ResultPopupVM(RoundResultData data, Popup popup)
            {
                Title = data.Title;
                Message = data.Message;
                CloseCommand = new Command(popup.Close);
            }        
    }
}
