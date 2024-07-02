using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01143 : CardPlot
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private SceneCORE3 SceneCORE3 => (SceneCORE3)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(_chaptersProvider.CurrentScene.DangerDiscardZone.Cards, _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true)
                .Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone).Execute();

            while (!_chaptersProvider.CurrentScene.DangerDeckZone.TopCard.Tags.Contains(Tag.Monster) ||
                _chaptersProvider.CurrentScene.DangerDeckZone.TopCard is not CardCreature)
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone.TopCard).Execute();

            CardCreature monster = (CardCreature)_chaptersProvider.CurrentScene.DangerDeckZone.TopCard;
            await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(monster, SceneCORE3.MainPath).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(monster.Eldritch, 1).Execute();
        }
    }
}
