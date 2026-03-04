using blackjack.ModelsLogic;
using blackjack.ViewModels;
using Firebase.Auth;
using System.Diagnostics;

namespace blackjack.Views;

public partial class GameTable : ContentPage
{
    private readonly GameTableVM vm;
    public GameTable(Game game)
	{
		InitializeComponent();
        vm = new GameTableVM(game);
        BindingContext = vm;  
    }
    protected override bool OnBackButtonPressed()
    {
        // Call your LeaveGame logic from VM
        if (BindingContext is GameTableVM vm)
        {
            vm.LeaveGame();
        }

        // Return true to **prevent default back navigation**
        // Return false if you want the page to pop normally as well
        return true;
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        vm.LeaveGame();
        base.OnNavigatedFrom(args);
    }

}