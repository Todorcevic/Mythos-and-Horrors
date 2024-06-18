using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Fast<T> : IReaction where T : GameAction
    {
        public GameActionTime Time => throw new System.NotImplementedException();

        public string Description => throw new System.NotImplementedException();

        /*******************************************************************/

        public Fast(PlayActionType playAction)
        {
        }

        /*******************************************************************/
        public Task React(GameAction gameAction)
        {
            throw new System.NotImplementedException();
        }

        public bool Check(GameAction gameAction, GameActionTime time)
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }
    }
}
