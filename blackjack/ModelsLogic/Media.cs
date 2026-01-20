
using blackjack.Models;

namespace blackjack.ModelsLogic
{
    public class Media : IMedia
    {
        public async Task<string?> PickImageAsync()
        {
            var result = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Select profile picture",
                    FileTypes = FilePickerFileType.Images
                });

            return result?.FullPath;
        }

        public async Task<string?> TakePhotoAsync()
        {
            if (!MediaPicker.Default.IsCaptureSupported)
                return null;

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null)
                return null;

            var localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using var stream = await photo.OpenReadAsync();
            using var newStream = File.OpenWrite(localPath);
            await stream.CopyToAsync(newStream);

            return localPath;
        }
    }
}
