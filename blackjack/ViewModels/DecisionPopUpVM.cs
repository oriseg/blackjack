
using blackjack.ModelsLogic;
using CommunityToolkit.Maui.Views;
using System;
using System.Reactive;
using System.Reflection.Metadata;
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
        }

        private void OnDouble()
        {
            game.Hit();

            // אם Bust – המשחק כבר העביר תור
                popup.Close();

        }

        private void OnStand()
        {
            game.Stand();
            popup.Close();
        }

        private void OnHit()
        {
            game.Double();
            popup.Close();
        }
    }
}
