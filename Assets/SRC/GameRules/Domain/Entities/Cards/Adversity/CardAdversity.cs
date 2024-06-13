using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public abstract Zone ZoneToMoveWhenDraw(Investigator investigator);

        public abstract Task PlayAdversityFor(Investigator investigator);
    }
}
