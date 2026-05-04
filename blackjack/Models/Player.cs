using blackjack.ModelsLogic;
using Plugin.CloudFirestore.Attributes;

namespace blackjack.Models
{
    public class Player
    {
        #region Fields
        #endregion

        #region Properties
        public string UserName { get; set; } = string.Empty;
        public Hand PlayerHand { get; set; } = new Hand();

        [Ignored]
        public bool IsCurrentTurn { get; set; }

        [Ignored]
        public double X { get; set; }

        [Ignored]
        public double Y { get; set; }
        #endregion

        #region Constructor
        public Player(string username)
        {
            UserName = username;
        }

        public Player()
        {
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}