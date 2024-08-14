using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class StartChapterGameAction : PhaseGameAction
    {
        public Chapter Chapter { get; private set; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_StartChapter");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_StartChapter");

        /*******************************************************************/
        public StartChapterGameAction SetWith(Chapter chapter)
        {
            Chapter = chapter;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Chapter.Description).Execute();
        }
    }
}
