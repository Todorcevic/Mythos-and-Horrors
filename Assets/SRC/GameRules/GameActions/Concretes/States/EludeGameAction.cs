using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EludeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardCreature Creature { get; }

        /*******************************************************************/
        public EludeGameAction(CardCreature creature)
        {
            Creature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Creature.Exausted, true));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, Creature.CurrentPlace.OwnZone));
        }
    }
}
