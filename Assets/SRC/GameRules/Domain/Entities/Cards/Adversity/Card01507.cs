using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card01507 : CardAdversity, IEndReactionable, IWeakness
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public async Task WhenFinish(GameAction gameAction)
        {
            if (CanActivate(gameAction))
            {
                await _gameActionFactory.Create(new MoveCardsGameAction(this, Owner.DangerZone)); //TODO Remove Resource
            }
        }

        private bool CanActivate(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.ToZone != moveCardsGameAction.ToZone.Owner?.HandZone) return false;

            return true;
        }
    }
}
