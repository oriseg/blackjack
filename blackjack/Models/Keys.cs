namespace blackjack.Models
{
    public class Keys
    {
        #region Firebase
        public const string FbApiKey = "AIzaSyBH-W6By77z-hX3_7iZ6z_B6YPEop3tqac";
        public const string FbAppDomainKey = "blackjack-3fdb3.firebaseapp.com";
        #endregion

        #region User Keys
        public const string NameKey = "NameKey";
        public const string EmailKey = "Email";
        public const string PasswordKey = "Password";
        public const string ProfileImageKey = "ProfileImage";
        #endregion

        #region Collections
        public const string GamesCollection = "Games";
        public const string UsersCollection = "Users";
        #endregion

        #region Error & Parsing
        public const string MessageKey = "message";
        public const string ErrorsKey = "errors";
        public const string WordsDelimiter = " ";
        public const string TitleDelimiter = "_";
        public const string ReasonKey = "reason";
        public const string UpperCaseDelimiter = "(?=[A-Z])";
        public const string NewLine = "\n";
        public const string Apostrophe = "'";
        public const string Colon = ":";
        public const string Comma = ",";
        #endregion

        #region UI / Layout
        public const int Width = 400;
        public const int Length = 600;
        #endregion

        #region Timer
        public const int FinishedSignal = -1;
        public const int TimerTotalTime = 10000;
        public const int TimerInterval = 1000;
        public const int TwoSecondDelay = 2000;
        #endregion

        #region Id
        public const int IdGenerator = 1000000;
        #endregion
    }
}