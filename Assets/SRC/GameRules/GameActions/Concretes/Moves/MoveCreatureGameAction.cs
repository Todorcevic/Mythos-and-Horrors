using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCreatureGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public CardCreature Creature { get; }
        public CardPlace Destiny { get; }

        /*******************************************************************/
        public MoveCreatureGameAction(CardCreature creature, CardPlace destiny)
        {
            Creature = creature;
            Destiny = destiny;
        }

        public MoveCreatureGameAction(IStalker creature)
        {
            Creature = (CardCreature)creature;
            Destiny = Creature.GetPlaceToStalkerMove();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Destiny == Creature.CurrentPlace) return;
            await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, Destiny.OwnZone));
        }
    }
}
