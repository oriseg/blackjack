using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using System.Windows.Input;

namespace blackjack.ViewModels;

public partial class RegisterPageVM : ObservableObject
{
    private readonly User user = new();
    private ImageSource? _capturedPhoto;

    public bool IsPassword { get; set; } = true;

    public ICommand RegisterCommand { get; }
    public ICommand ToggleIsPasswordCommand { get; }
    public ICommand TakePhotoCommand { get; }

    public RegisterPageVM()
    {
        user.OnRegAuthComplete += User_OnRegAuthComplete;

        RegisterCommand = new Command(Register, CanRegister);

        ToggleIsPasswordCommand = new Command(() =>
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        });

        TakePhotoCommand = new Command(async () =>
        {
            CameraPage cameraPage = new CameraPage();

            await Application.Current!.MainPage!.Navigation.PushModalAsync(cameraPage);

            cameraPage.Disappearing += (s, e) =>
            {
                if (cameraPage.CapturedPhoto != null)
                {
                    _capturedPhoto = cameraPage.CapturedPhoto;
                    OnPropertyChanged(nameof(ProfileImage));
                }
            };
        });

    }

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

    public ImageSource? ProfileImage => _capturedPhoto;

    private bool CanRegister() =>
        !string.IsNullOrWhiteSpace(UserName) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(Email);

    private void Register()
    {
        user.Register();
    }
}