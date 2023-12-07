using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly AdventurersProvider _adventurersProvider;

        /*******************************************************************/
        public async Task Run() => await Start();

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<PrepareAdventurerGameAction>().Run(_adventurersProvider.AllAdventurers[0]);
            //await _gameActionFactory.Create<PrepareAdventurerGameAction>().Run(_adventurersProvider.AllAdventurers[1]);
            //await _gameActionFactory.Create<PrepareAdventurerGameAction>().Run(_adventurersProvider.AllAdventurers[2]);

 
        }
    }
}
