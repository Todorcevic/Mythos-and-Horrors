using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureConfrontAttackGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CreatureConfrontAttackGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CreatureConfrontAttackGameAction);
        public override Phase MainPhase => Phase.Creature;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {

            var asda = _cardsProvider.AttackerCreatures.ToList();
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(AttackerCreatures, Attack).Execute();

            /*******************************************************************/
            IEnumerable<CardCreature> AttackerCreatures() => _cardsProvider.AttackerCreatures;

            async Task Attack(CardCreature cardCreature)
            {
                if (cardCreature is CardColosus colosus)
                    await _gameActionsProvider.Create<ColosusAttackGameAction>().SetWith(colosus).Execute();
                else await _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cardCreature, cardCreature.ConfrontedInvestigator).Execute();
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(cardCreature.Exausted, true).Execute();
            }
        }
    }
}
