using blackjack.ViewModels;

namespace blackjack.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		try
		{
            InitializeComponent();
        }
		catch(Exception ex)
		{
            Console.WriteLine(ex.Message);
        }
        
        BindingContext = new LogInPageVM();
    }
}