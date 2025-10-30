using CommunityToolkit.Maui.Views;
namespace blackjack.Views; 


public partial class JoinPopup : Popup
{
	public JoinPopup()
	{
		InitializeComponent();
        //Popup.Color controls the background color behind your popup’s content
        this.Color = Colors.Transparent;
    }
}