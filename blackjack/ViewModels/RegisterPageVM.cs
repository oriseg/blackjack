using blackjack.Models;
using blackjack.ModelsLogic;
using blackjack.Views;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace blackjack.ViewModels;

public class RegisterPageVM : ObservableObject
{
    private readonly User user = new();
    private ImageSource? _capturedPhoto;

    public bool IsPassword { get; set; } = true;

    public ICommand RegisterCommand { get; }
    public ICommand ToggleIsPasswordCommand { get; }
    public ICommand TakePhotoCommand { get; }

    public RegisterPageVM()
    {
        RegisterCommand = new Command(Register, CanRegister);
        ToggleIsPasswordCommand = new Command(() =>
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        });

        TakePhotoCommand = new Command(async () =>
        {
            var cameraPage = new CameraPage();
            await Application.Current!.MainPage!.Navigation.PushModalAsync(cameraPage);

            cameraPage.Disappearing += (s, e) =>
            {
                if (cameraPage.CameraVM.CapturedPhoto != null)
                {
                    _capturedPhoto = cameraPage.CameraVM.CapturedPhoto;
                    OnPropertyChanged(nameof(ProfileImage));
                }
            };
        });
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

    public ImageSource? ProfileImage => _capturedPhoto ??
        (!string.IsNullOrEmpty(user.ProfileImagePath) ? ImageSource.FromFile(user.ProfileImagePath) : null);

    private bool CanRegister() =>
        !string.IsNullOrWhiteSpace(UserName) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(Email);

    private void Register()
    {
        user.Register();
    }
}
