using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new StartChapterGameAction(_chaptersProvider.CurrentChapter));

            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                await _gameActionFactory.Create(new PrepareInvestigatorGameAction(investigator));
            }

            await _gameActionFactory.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            while (true)
            {
                await _gameActionFactory.Create(new InvestigatorsPhaseGameAction());
                await _gameActionFactory.Create(new CreaturePhaseGameAction());
                await _gameActionFactory.Create(new RestorePhaseGameAction());
                await _gameActionFactory.Create(new ScenePhaseGameAction());
            }
        }
    }
}
