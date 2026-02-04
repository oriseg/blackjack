using blackjack.Models;
using blackjack.ViewModels;
using CommunityToolkit.Maui.Views;

namespace blackjack.Views;

public partial class ResultPopup : Popup
{
	public ResultPopup(RoundResultData data)
	{
		InitializeComponent();
        BindingContext = new ResultPopupVM(data, this);
    }
}