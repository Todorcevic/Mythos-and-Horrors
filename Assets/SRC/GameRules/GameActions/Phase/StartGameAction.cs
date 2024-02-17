using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                await _gameActionFactory.Create(new PrepareInvestigatorGameAction(investigator));
            }

            await _gameActionFactory.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            while (true)
            {
                await _gameActionFactory.Create(new InvestigatorPhaseGameAction());
                await _gameActionFactory.Create(new CreaturePhaseGameAction());
                await _gameActionFactory.Create(new RestorePhaseGameAction());
                await _gameActionFactory.Create(new ScenePhaseGameAction());
            }
        }
    }
}
