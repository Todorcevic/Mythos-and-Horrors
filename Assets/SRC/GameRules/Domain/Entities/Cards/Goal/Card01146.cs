using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01146 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;

        private SceneCORE3 SceneCORE3 => (SceneCORE3)_chapterProvider.CurrentScene;

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            if (!SceneCORE3.Ritual.IsInPlay)
                await _gameActionsProvider.Create<MoveCardsGameAction>()
                    .SetWith(SceneCORE3.Ritual, _chapterProvider.CurrentScene.GetPlaceZone(1, 4)).Execute();
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(SceneCORE3.CultistsNotInterrogate, Spawn).Execute();

            /*******************************************************************/
            async Task Spawn(CardCreature creature) =>
              await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(creature, SceneCORE3.MainPath).Execute();
        }
    }
}
