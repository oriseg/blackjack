using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using System.Windows.Input;

namespace blackjack.ViewModels;

public partial class RegisterPageVM : ObservableObject
{
    #region Fields
    private readonly User user = new();
    #endregion


    #region Commands
    public ICommand RegisterCommand { get; }
    public ICommand ToggleIsPasswordCommand { get; }
    #endregion

    #region Properties
    public bool IsPassword { get; set; } = true;

    public string UserName
    {
        get => user.UserName;
        set { user.UserName = value; (RegisterCommand as Command)?.ChangeCanExecute(); }
    }

    public string Password
    {
        get => user.Password;
        set { user.Password = value; (RegisterCommand as Command)?.ChangeCanExecute(); }
    }

    public string Email
    {
        get => user.Email;
        set { user.Email = value; (RegisterCommand as Command)?.ChangeCanExecute(); }
    }

    #endregion

    #region Constructor
    public RegisterPageVM()
    {
        user.OnRegAuthComplete += User_OnRegAuthComplete;

        RegisterCommand = new Command(Register, CanRegister);

        ToggleIsPasswordCommand = new Command(() =>
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        });
    }
    #endregion


    #region Private Methods
    private void User_OnRegAuthComplete(object? sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current.MainPage = new LoginPage();
            });
        }
    }

    private bool CanRegister() =>
        !string.IsNullOrWhiteSpace(UserName) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(Email);

    private void Register()
    {
        user.Register();
    }
    #endregion
}