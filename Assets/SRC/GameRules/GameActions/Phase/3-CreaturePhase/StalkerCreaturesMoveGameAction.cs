using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //3.2	Hunter enemies move.
    public class StalkerCreaturesMoveGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public IStalker StalkerToMove { get; private set; }
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(StalkerCreaturesMoveGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(StalkerCreaturesMoveGameAction);
        public override Phase MainPhase => Phase.Creature;
        public IStalker NextStalker => _cardsProvider.StalkersInPlay.NextElementFor(StalkerToMove);
        public override bool CanBeExecuted => StalkerToMove != null;

        /*******************************************************************/
        public StalkerCreaturesMoveGameAction(IStalker stalker)
        {
            StalkerToMove = stalker;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new MoveCreatureGameAction(StalkerToMove));
            await _gameActionsProvider.Create(new StalkerCreaturesMoveGameAction(NextStalker));
        }
    }
}
