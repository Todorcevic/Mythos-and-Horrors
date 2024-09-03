using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class StartChapterGameAction : PhaseGameAction
    {
        public Chapter Chapter { get; private set; }
        public override Phase MainPhase => Phase.Prepare;
        public override Localization PhaseNameLocalization => new("PhaseName_StartChapter");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_StartChapter");

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
