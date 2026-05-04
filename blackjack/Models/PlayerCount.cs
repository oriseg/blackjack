using Plugin.CloudFirestore.Attributes;

namespace blackjack.Models
{
    public class PlayerCount
    {


        #region Properties
        public int Count { get; set; }
        [Ignored]
        public string DisplayName => $"{Count}";
        #endregion

        #region Constructor
        public PlayerCount(int count)
        {
            Count = count;
        }

        public PlayerCount()
        {
        }
        #endregion
    }
}