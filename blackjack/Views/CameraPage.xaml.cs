using Camera.MAUI;

namespace blackjack.Views;

public partial class CameraPage : ContentPage
{
    public ImageSource? CapturedPhoto { get; private set; }

    public CameraPage()
    {
        InitializeComponent();
        Loaded += CameraPage_Loaded!;
    }

    private async void CameraPage_Loaded(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            return;

        if (CameraView.NumCamerasDetected > 0)
        {
            CameraView.Camera = CameraView.Cameras.FirstOrDefault(c => c.Position == CameraPosition.Back) ?? CameraView.Cameras.First();
        }

        await CameraView.StartCameraAsync();
        await Task.Delay(300); // allow hardware to initialize
    }

    private async void TakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            Stream stream = await CameraView.TakePhotoAsync();
            if (stream == null)
                return;

            MemoryStream memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            memory.Position = 0;

            CapturedPhoto = ImageSource.FromStream(() => memory);

            await CameraView.StopCameraAsync();
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await CameraView.StopCameraAsync();
    }
}