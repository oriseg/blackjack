using blackjack.Models;

namespace blackjack.ModelsLogic
{
    internal class User : UserModel
    {
        public override bool Login()
        {
            return true;
        }
        public override bool Register()
        {
          return true ;
        }
       
    }
}
