
using blackjack.Models;
using blackjack.Views;
using System.Windows.Input;

namespace blackjack.ViewModels
{
    public class ProfilePageVM : ObservableObject
    {
        private ImageSource? profileImage;

        public ImageSource? ProfileImage
        {
            get => profileImage;
            set
            {
                profileImage = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ProfilePageVM()
        {
            ProfileImage = null;
            SaveCommand = new Command(Save);
            TakePhotoCommand = new Command(async () =>
            {
                CameraPage cameraPage = new CameraPage();

                await Application.Current!.MainPage!.Navigation.PushModalAsync(cameraPage);

                cameraPage.Disappearing += (s, e) =>
                {
                    if (cameraPage.CapturedPhoto != null)
                    {
                        ProfileImage = cameraPage.CapturedPhoto;
                        OnPropertyChanged(nameof(ProfileImage));
                    }
                };
            });
        }
        private async void Save()
        {
            await Shell.Current.Navigation.PopAsync();
        }


    }
}
