using blackjack.ViewModels;

namespace blackjack.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		BindingContext = new ProfilePageVM();
	}
}