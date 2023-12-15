using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public async Task Run() => await Start();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<ShowHistoryGameAction>().Run(_chaptersProvider.CurrentChapter.Description);

            foreach (Adventurer adventurer in _adventurersProvider.AllAdventurers)
            {
                await _gameActionFactory.Create<PrepareAdventurerGameAction>().Run(adventurer);
            }

            await _gameActionFactory.Create<PrepareSceneGameAction>().Run(_chaptersProvider.CurrentScene);
            await _gameActionFactory.Create<PlayGameAction>().Run();
        }
    }
}
