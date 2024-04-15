using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new StartChapterGameAction(_chaptersProvider.CurrentChapter));
            await new SafeForeach<Investigator>(Prepare, GetInvestigators).Execute();
            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            await _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
            await _gameActionsProvider.Create(new CreaturePhaseGameAction());
            await _gameActionsProvider.Create(new RestorePhaseGameAction());
            while (true)
            {
                await _gameActionsProvider.Create(new ScenePhaseGameAction());
                await _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
                await _gameActionsProvider.Create(new CreaturePhaseGameAction());
                await _gameActionsProvider.Create(new RestorePhaseGameAction());
            };
        }

        private async Task Prepare(Investigator investigator) =>
            await _gameActionsProvider.Create(new PrepareInvestigatorGameAction(investigator));

        private IEnumerable<Investigator> GetInvestigators() => _investigatorsProvider.AllInvestigators;
    }
}
