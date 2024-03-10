using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01507 : CardAdversity, IWeakness
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            if (CanActivate(gameAction)) //TODO Make Reaction
            {
                await _gameActionFactory.Create(new MoveCardsGameAction(this, Owner.DangerZone)); //TODO Remove Resource
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
