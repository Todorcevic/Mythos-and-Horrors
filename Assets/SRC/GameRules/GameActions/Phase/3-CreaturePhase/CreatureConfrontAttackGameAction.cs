using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureConfrontAttackGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public CardCreature CreatureAttacker { get; private set; }

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CreatureConfrontAttackGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CreatureConfrontAttackGameAction);
        public override Phase MainPhase => Phase.Creature;

        /*******************************************************************/
        public CardCreature NextCreatureAttacker => _cardsProvider.AttackerCreatures.NextElementFor(CreatureAttacker);
        public override bool CanBeExecuted => CreatureAttacker != null;

        /*******************************************************************/
        public CreatureConfrontAttackGameAction(CardCreature cardCreature)
        {
            CreatureAttacker = cardCreature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new CreatureAttackGameAction(CreatureAttacker, CreatureAttacker.ConfrontedInvestigator));
            await _gameActionsProvider.Create(new CreatureConfrontAttackGameAction(NextCreatureAttacker));
        }
    }
}
