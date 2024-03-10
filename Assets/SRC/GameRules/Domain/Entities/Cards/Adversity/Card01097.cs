using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01097 : CardAdversity, IWeakness
    {
        [Inject] private readonly GameActionProvider _gameActionRepository;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            if (CanActivate(gameAction)) //TODO Make Reaction
            {
                await _gameActionRepository.Create(new MoveCardsGameAction(this, null)); //TODO Remove Resource
            }

            await base.WhenFinish(gameAction);
        }

        /*******************************************************************/
        private bool CanActivate(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != moveCardsGameAction.ToZone.Owner?.HandZone) return false;

            return true;
        }
    }
}
