using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules.News
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
