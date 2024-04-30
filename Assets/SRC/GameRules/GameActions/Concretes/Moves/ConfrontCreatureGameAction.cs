using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ConfrontCreatureGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public CardCreature Creature { get; }

        /*******************************************************************/
        public ConfrontCreatureGameAction(CardCreature creature)
        {
            Creature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            IEnumerable<Investigator> investigators = _investigatorProvider.GetInvestigatorsInThisPlace(Creature.CurrentPlace);
            Investigator investigator = investigators.First();
            if (Creature is ITarget target && investigators.Contains(target.Investigator)) investigator = target.Investigator;

            await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, investigator.DangerZone));
        }
    }
}
