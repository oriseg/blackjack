
using blackjack.ModelsLogic;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public class DecisionPopUpVM
    {
        private readonly Game game;
        private readonly Popup popup;

        public ICommand HitCommand { get; }
        public ICommand StandCommand { get; }
        public ICommand DoubleCommand { get; }
        public DecisionPopUpVM(Game game, Popup popup)
        {
            this.game = game;
            this.popup = popup;
            HitCommand = new Command(OnHit);
            StandCommand = new Command(OnStand);
            DoubleCommand = new Command(OnDouble);
            game.Onbust += Bust;
        }

        private void Bust(object? sender, EventArgs e)
        {
            ClosePopUp();
        }

        private void OnDouble()
        {
            game.Hit();
            ClosePopUp();
        }

        private void OnStand()
        {
            game.Stand();
            ClosePopUp();
        }

        private void OnHit()
        {
            game.Hit();          
        } 
        public void ClosePopUp()
        {
            popup.Close();
        }
    }
}
