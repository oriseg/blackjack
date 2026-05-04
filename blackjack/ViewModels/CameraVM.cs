using blackjack.Models;
using Camera.MAUI;
using System.Windows.Input;

namespace blackjack.ViewModels;

public partial class CameraVM : ObservableObject
{
    #region Fields
    private ImageSource? _capturedPhoto;
    private readonly CameraView _cameraView;
    #endregion

    #region Commands
    public ICommand TakePhotoCommand { get; }
    #endregion

    #region Properties
    public ImageSource? CapturedPhoto
    {
        get => _capturedPhoto;
        set
        {
            _capturedPhoto = value;
            OnPropertyChanged(nameof(CapturedPhoto));
        }
    }
    #endregion

    #region Constructor
    public CameraVM(CameraView cameraView)
    {
        _cameraView = cameraView;
        TakePhotoCommand = new Command(async () => await TakePhoto());
    }
    #endregion

    #region Private Methods
    private async Task TakePhoto()
    {
        try
        {
            Stream stream = await _cameraView.TakePhotoAsync();
            if (stream != null)
                CapturedPhoto = ImageSource.FromStream(() => stream);
        }
        catch (Exception ex)
        {
            await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
    }
    #endregion
}