using blackjack.ViewModels;
using Camera.MAUI;

namespace blackjack.Views;

public partial class CameraPage : ContentPage
{
    public CameraVM CameraVM { get; private set; }

    public CameraPage()
    {
        InitializeComponent();
        CameraVM = new CameraVM(CameraView);
        BindingContext = CameraVM;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            return;

        await CameraView.StartCameraAsync();
        Console.WriteLine("Detected: " + CameraView.NumCamerasDetected);

        if (CameraView.NumCamerasDetected > 0)
        {
            CameraView.Camera = CameraView.Cameras.FirstOrDefault(c => c.Position == CameraPosition.Back) ?? CameraView.Cameras.First();
        }
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CameraView.StopCameraAsync();
    }
}
