using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new StartChapterGameAction(_chaptersProvider.CurrentChapter));
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigators, Prepare).Start();
            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            while (true) await _gameActionsProvider.Create(new RoundGameAction());
        }

        /*******************************************************************/
        private IEnumerable<Investigator> AllInvestigators() => _investigatorsProvider.AllInvestigators;

        private async Task Prepare(Investigator investigator) =>
            await _gameActionsProvider.Create(new PrepareInvestigatorGameAction(investigator));
    }
}
