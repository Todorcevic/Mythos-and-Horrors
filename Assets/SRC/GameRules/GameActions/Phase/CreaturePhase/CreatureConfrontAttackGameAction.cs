using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //3.3	Next investigator resolves engaged enemy attacks.
    public class CreatureConfrontAttackGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CreatureConfrontAttackGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CreatureConfrontAttackGameAction);
        public override Phase MainPhase => Phase.Creature;
        private IEnumerable<CardCreature> AttackerCreatures => _cardsProvider.AllCards.OfType<CardCreature>()
                  .Where(creature => creature.IsConfronted && !creature.Exausted.IsActive)
                  .OrderBy(creature => creature.ConfrontedInvestigator.Position);

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (CardCreature creature in AttackerCreatures)
            {
                await _gameActionFactory.Create(new CreatureAttackGameAction(creature, creature.ConfrontedInvestigator));
            }
        }
    }
}
