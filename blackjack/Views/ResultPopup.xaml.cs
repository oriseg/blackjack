using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.ViewModels;
using CommunityToolkit.Maui.Views;

namespace blackjack.Views;

public partial class ResultPopup : Popup
{
	public ResultPopup(RoundResultData data , Game game)
	{
		InitializeComponent();
        BindingContext = new ResultPopupVM(data,game,this);
    }
}