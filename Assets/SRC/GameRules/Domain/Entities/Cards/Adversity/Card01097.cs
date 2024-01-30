using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{

    public class Card01097 : CardAdversity, IEndReactionable, IWeakness
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public async Task WhenFinish(GameAction gameAction)
        {
            if (CanActivate(gameAction))
            {
                await _gameActionRepository.Create(new MoveCardsGameAction(this, null)); //TODO Remove Resource
            }
        }

        private bool CanActivate(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.Zone != _investigatorsProvider.GetInvestigatorWithThisZone(moveCardsGameAction.Zone)?.HandZone) return false;

            return true;
        }
    }
}
