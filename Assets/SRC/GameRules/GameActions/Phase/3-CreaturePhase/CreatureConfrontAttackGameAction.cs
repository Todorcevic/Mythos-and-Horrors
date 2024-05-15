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
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CreatureConfrontAttackGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CreatureConfrontAttackGameAction);
        public override Phase MainPhase => Phase.Creature;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<CardCreature>(() => _cardsProvider.AttackerCreatures, Attack));

            /*******************************************************************/
            async Task Attack(CardCreature cardCreature) =>
                await _gameActionsProvider.Create(new CreatureAttackGameAction(cardCreature, cardCreature.ConfrontedInvestigator));
        }
    }
}
