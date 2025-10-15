using blackjack.ViewModels;

namespace blackjack.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomePageVM();
    }
}