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
    private async void CopyIdClicked(object sender, EventArgs e)
    {
        if (BindingContext is GameTableVM vm)
        {
            await Clipboard.SetTextAsync(vm.Id);
            await DisplayAlert("Copied", "Game ID copied!", "OK");
        }
    }
}