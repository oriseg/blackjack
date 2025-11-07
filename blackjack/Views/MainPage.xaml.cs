using blackjack.ViewModels;

namespace blackjack.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM();
        }

        private void Picker_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
