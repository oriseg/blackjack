
using CommunityToolkit.Maui.Views;
using blackjack.ViewModels;
using blackjack.ModelsLogic;
namespace blackjack.Views;

public partial class DecisionPopUp : Popup
{
	public DecisionPopUp(Game game) 
	{
		InitializeComponent(); 
		BindingContext = new DecisionPopUpVM(game,this);
    }
}