using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class StartChapterGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public Chapter Chapter { get; private set; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.START_CHAPTER_PHASE_NAME;
        public override string Description => _textsProvider.GameText.START_CHAPTER_PHASE_DESCRIPTION;

        /*******************************************************************/
        public StartChapterGameAction SetWith(Chapter chapter)
        {
            Chapter = chapter;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Chapter.Description).Start();
        }
    }
}
