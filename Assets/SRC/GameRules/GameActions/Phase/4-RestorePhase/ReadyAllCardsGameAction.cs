using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReadyAllCardsGameAction : PhaseGameAction
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ReadyAllCardsGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ReadyAllCardsGameAction);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new ReadyCardGameAction(_cardsProvider.GetCardsExhausted()));
        }
    }
}
