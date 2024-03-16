using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureConfrontAttackGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CreatureConfrontAttackGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CreatureConfrontAttackGameAction);
        public override Phase MainPhase => Phase.Creature;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (CardCreature creature in _cardsProvider.AttackerCreatures)
            {
                await _gameActionFactory.Create(new CreatureAttackGameAction(creature, creature.ConfrontedInvestigator));
            }
        }
    }
}
