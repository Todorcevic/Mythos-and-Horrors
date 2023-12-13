using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public abstract class Buff : IStartReactionable, IEndReactionable
    {
        public List<Card> CardsAffected { get; } = new List<Card>();

        /*******************************************************************/
        public virtual Task Apply()
        {
            foreach (Card card in CardsAffected)
            {

            }

            return Task.CompletedTask;
        }


        public abstract Task Remove();

        public Task WhenBegin(GameAction gameAction)
        {
            throw new System.NotImplementedException();
        }

        public Task WhenFinish(GameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
