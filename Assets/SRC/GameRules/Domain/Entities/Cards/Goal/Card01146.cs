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
                await _gameActionsProvider.Create(new MoveCardsGameAction(SceneCORE3.Ritual, _chapterProvider.CurrentScene.GetPlaceZone(1, 4)));
            await _gameActionsProvider.Create(new SafeForeach<CardCreature>(SceneCORE3.CultistsNotInterrogate, Spawn));

            /*******************************************************************/
            async Task Spawn(CardCreature creature) =>
              await _gameActionsProvider.Create(new SpawnCreatureGameAction(creature, SceneCORE3.MainPath));
        }
    }
}
