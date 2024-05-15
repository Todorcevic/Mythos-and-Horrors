using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OpportunityAttackGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }

        public override bool CanBeExecuted => Investigator.CreaturesEnganged.Any();

        /*******************************************************************/
        public OpportunityAttackGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<CardCreature>(() => Investigator.CreaturesEnganged, CreatureAttack));

            async Task CreatureAttack(CardCreature creature) =>
                await _gameActionsProvider.Create(new CreatureAttackGameAction(creature, Investigator));

        }
    }
}
