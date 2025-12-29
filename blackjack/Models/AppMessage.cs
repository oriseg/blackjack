using CommunityToolkit.Mvvm.Messaging.Messages;

namespace blackjack.Models
{
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {

    }
}
