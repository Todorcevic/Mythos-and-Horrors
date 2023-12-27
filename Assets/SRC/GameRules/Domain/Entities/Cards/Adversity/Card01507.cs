using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card01507 : CardAdversity, IEndReactionable, IWeakness
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly AdventurersProvider _adventurersProvider;

        /*******************************************************************/
        public async Task WhenFinish(GameAction gameAction)
        {
            if (CanActivate(gameAction))
            {
                await _gameActionRepository.Create<MoveCardsGameAction>().Run(this, _adventurersProvider.GetAdventurerWithThisCard(this).DangerZone); //TODO Remove Resource
            }
        }

        private bool CanActivate(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.Zone != _adventurersProvider.GetAdventurerWithThisZone(moveCardsGameAction.Zone)?.HandZone) return false;

            return true;
        }
    }
}
