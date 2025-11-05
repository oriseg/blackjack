
using blackjack.Models;

namespace blackjack.ModelsLogic
{
    internal class Game : GameModel
    { 
        internal Game()
        {
            HostName = new User().UserName; 
            Created = DateTime.Now; 
            
        } 
        internal void crateGame()
        {
            Game game = new Game(); 
            game.SetDocument(OnComplete);
        }
        private void OnComplete(Task task)
        {
         
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
        }
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete); 


        }
    }
}
