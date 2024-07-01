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
            await _gameActionsProvider.Create(new SafeForeach<CardCreature>(() => _cardsProvider.AttackerCreatures, Attack));

            /*******************************************************************/
            async Task Attack(CardCreature cardCreature)
            {
                if (cardCreature is CardColosus colosus)
                    await _gameActionsProvider.Create(new ColosusAttackGameAction(colosus));
                else await _gameActionsProvider.Create(new CreatureAttackGameAction(cardCreature, cardCreature.ConfrontedInvestigator));
                await _gameActionsProvider.Create(new UpdateStatesGameAction(cardCreature.Exausted, true));
            }
        }
    }
}
