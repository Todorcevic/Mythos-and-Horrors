using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public virtual async Task PlayAdversityFor(Investigator investigator)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, investigator.DangerZone));

        }
    }
}
