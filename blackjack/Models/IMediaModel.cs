namespace blackjack.Models
{
        public interface IMedia
        {
            Task<string?> PickImageAsync();
            Task<string?> TakePhotoAsync();
        }
}
