using blackjack.ModelsLogic;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public class DecisionPopUpVM
    {
        #region Fields
        private readonly Game game;
        private readonly Popup popup;
        #endregion

        #region Commands
        public ICommand HitCommand { get; }
        public ICommand StandCommand { get; }
        public ICommand DoubleCommand { get; }
        #endregion

        #region Constructor
        public DecisionPopUpVM(Game game, Popup popup)
        {
            this.game = game;
            this.popup = popup;

            HitCommand = new Command(OnHit);
            StandCommand = new Command(OnStand);
            DoubleCommand = new Command(OnDouble);

            game.Onbust += Bust;
        }
        #endregion

        #region Public Methods
        public void ClosePopUp()
        {
            popup.Close();
            game.Onbust -= Bust;
        }
        #endregion

        #region Private Methods
        private void Bust(object? sender, EventArgs e)
        {
            ClosePopUp();
        }

        private void OnDouble()
        {
            game.Double();
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
        #endregion
    }
}