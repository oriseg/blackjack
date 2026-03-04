
namespace blackjack.Models
{
    public class RoundResultData
    {
        public string TargetUserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public RoundOutcome Outcome { get; set; }
 
    }
    public enum RoundOutcome
    {
        Win,
        Lose,
        Push
    }
}

