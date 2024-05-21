using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01143 : CardPlot
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        SceneCORE3 SceneCORE3 => (SceneCORE3)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.DangerDiscardZone.Cards,
                           _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(_chaptersProvider.CurrentScene.DangerDeckZone));

            while (!_chaptersProvider.CurrentScene.DangerDeckZone.TopCard.Tags.Contains(Tag.Monster) ||
                _chaptersProvider.CurrentScene.DangerDeckZone.TopCard is not CardCreature)
                await _gameActionsProvider.Create(new DiscardGameAction(_chaptersProvider.CurrentScene.DangerDeckZone.TopCard));

            CardCreature monster = (CardCreature)_chaptersProvider.CurrentScene.DangerDeckZone.TopCard;
            await _gameActionsProvider.Create(new SpawnCreatureGameAction(monster, SceneCORE3.MainPath));
            await _gameActionsProvider.Create(new IncrementStatGameAction(monster.Eldritch, 1));
        }
    }
}
