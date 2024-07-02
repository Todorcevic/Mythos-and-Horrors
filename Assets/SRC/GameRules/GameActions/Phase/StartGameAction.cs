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
            await _gameActionsProvider.Create<StartChapterGameAction>().SetWith(_chaptersProvider.CurrentChapter).Start();
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigators, Prepare).Start();
            await _gameActionsProvider.Create<PrepareSceneGameAction>().SetWith(_chaptersProvider.CurrentScene).Start();

            while (true) await _gameActionsProvider.Create<RoundGameAction>().Start();
        }

        /*******************************************************************/
        private IEnumerable<Investigator> AllInvestigators() => _investigatorsProvider.AllInvestigators;

        private async Task Prepare(Investigator investigator) =>
            await _gameActionsProvider.Create<PrepareInvestigatorGameAction>().SetWith(investigator).Start();
    }
}
