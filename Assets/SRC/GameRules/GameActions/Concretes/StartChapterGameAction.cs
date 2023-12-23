using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartChapterGameAction : GameAction
    {
        private ChapterInfo _chapter;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public async Task Run(ChapterInfo chapter)
        {
            _chapter = chapter;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<ShowHistoryGameAction>().Run(_chapter.Description);
        }
    }
}
