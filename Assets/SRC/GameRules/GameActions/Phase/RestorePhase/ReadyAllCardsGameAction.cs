using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //4.3	Ready all exhausted cards.
    public class ReadyAllCardsGameAction : PhaseGameAction
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ReadyAllCardsGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ReadyAllCardsGameAction);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Card card in _cardsProvider.GetCardsExhausted())
            {
                await _gameActionProvider.Create(new ReadyCardGameAction(card));
            }
        }
    }
}
