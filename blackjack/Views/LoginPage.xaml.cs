using blackjack.ViewModels;

namespace blackjack.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LogInPageVM();
    }
}