using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ConfrontCreatureGameAction : GameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public CardCreature Creature { get; private set; }
        public Investigator ConfrontedInvestigator { get; private set; }

        /*******************************************************************/
        public ConfrontCreatureGameAction SetWith(CardCreature creature, Investigator confrontedInvestigator = null)
        {
            Creature = creature;
            ConfrontedInvestigator = confrontedInvestigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Investigator investigator = ConfrontedInvestigator ?? _investigatorProvider.GetInvestigatorsInThisPlace(Creature.CurrentPlace).FirstOrDefault();

            if (Creature is ITarget target && _investigatorProvider.GetInvestigatorsInThisPlace(Creature.CurrentPlace)
                .Contains(target.TargetInvestigator)) investigator = target.TargetInvestigator;

            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(Creature, investigator.DangerZone).Execute();
        }
    }
}
