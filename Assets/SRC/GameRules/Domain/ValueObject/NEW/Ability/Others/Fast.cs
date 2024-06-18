using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Fast<T> : IRealReaction where T : GameAction
    {
        public GameActionTime Time => throw new System.NotImplementedException();

        /*******************************************************************/

        public Fast(PlayActionType playAction)
        {
        }

        /*******************************************************************/
        public Task React(GameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
