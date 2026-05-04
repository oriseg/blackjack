namespace blackjack.Models
{
    public class TimerSettings
    {
        #region Properties
        public long TotalTimeInMilliseconds { get; set; }
        public long IntervalInMilliseconds { get; set; }
        #endregion

        #region Constructor
        public TimerSettings(long totalTimeInMilliseconds, long intervalInMilliseconds)
        {
            TotalTimeInMilliseconds = totalTimeInMilliseconds;
            IntervalInMilliseconds = intervalInMilliseconds;
        }
        #endregion
    }
}