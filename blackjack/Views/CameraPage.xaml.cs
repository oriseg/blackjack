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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CameraView.StartCameraAsync(); // ?? This is required
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CameraView.StopCameraAsync();
    }
}
