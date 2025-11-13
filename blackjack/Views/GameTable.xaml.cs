using blackjack.ModelsLogic;
using blackjack.ViewModels;

namespace blackjack.Views;

public partial class GameTable : ContentPage
{
	public GameTable(Game game)
	{
		InitializeComponent();
		BindingContext = new GameTableVM(game);
	}
}