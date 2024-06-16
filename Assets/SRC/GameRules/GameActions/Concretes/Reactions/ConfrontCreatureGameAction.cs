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
        public Investigator ConfrontedInvestigator { get; }

        /*******************************************************************/
        public ConfrontCreatureGameAction(CardCreature creature, Investigator confrontedInvestigator = null)
        {
            Creature = creature;
            ConfrontedInvestigator = confrontedInvestigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator investigator = ConfrontedInvestigator ?? _investigatorProvider.GetInvestigatorsInThisPlace(Creature.CurrentPlace).FirstOrDefault();

            if (Creature is ITarget target && _investigatorProvider.GetInvestigatorsInThisPlace(Creature.CurrentPlace)
                .Contains(target.TargetInvestigator)) investigator = target.TargetInvestigator;

            await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, investigator.DangerZone));
        }
    }
}
