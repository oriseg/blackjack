using blackjack.Models;
using blackjack.ModelsLogic;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

public class ResultPopupVM : BindableObject
{
    private readonly Game game;
    private readonly Popup popup;

    private string? countdownMessage;
    public string? CountdownMessage
    {
        get => countdownMessage;
        set
        {
            countdownMessage = value;
            OnPropertyChanged();
        }
    }

    public string Title { get; }
    public string Message { get; }

    public ICommand CloseCommand { get; }

    public ResultPopupVM(RoundResultData data, Game game, Popup popup)
    {
        Title = data.Title;
        Message = data.Message;
        this.game = game;
        this.popup = popup;
        CloseCommand = new Command(() => popup.Close());
        StartCountdown();
    }

    private async void StartCountdown()
    {
        int countdown = 5; 
        while (countdown > 0)
        {
            CountdownMessage = $"Next round starting in {countdown}...";
            await Task.Delay(1000); 
            countdown--;
        }

        CountdownMessage = "Starting next round...";
        await Task.Delay(500); 
        popup.Close();
    }
}
