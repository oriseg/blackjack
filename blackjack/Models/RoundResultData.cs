namespace blackjack.Models
{
    #region RoundResultData Class
    public class RoundResultData
    {
        #region Properties
        public string TargetUserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public RoundOutcome Outcome { get; set; }
        #endregion
    }
    #endregion

    #region RoundOutcome Enum
    public enum RoundOutcome
    {
        Win,
        Lose,
        Push
    }
    #endregion
}