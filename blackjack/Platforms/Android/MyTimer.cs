
using Android.OS;
using blackjack.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace blackjack.Platforms.Android
{
        public class MyTimer(long millisInFuture, long countDownInterval) : CountDownTimer(millisInFuture, countDownInterval)
        {
            public override void OnFinish()
            {
                WeakReferenceMessenger.Default.Send(new AppMessage<long>(Keys.FinishedSignal));
            }

            public override void OnTick(long millisUntilFinished)
            {
                WeakReferenceMessenger.Default.Send(new AppMessage<long>(millisUntilFinished));
            }
        }
}
