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

            foreach (Investigator investigator in _investigatorsProvider.Investigators)
            {
                await _gameActionsProvider.Create(new PrepareInvestigatorGameAction(investigator));
            }

            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            while (true)
            {
                await _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
                await _gameActionsProvider.Create(new CreaturePhaseGameAction());
                await _gameActionsProvider.Create(new RestorePhaseGameAction());
                await _gameActionsProvider.Create(new ScenePhaseGameAction());
            }
        }
    }
}
