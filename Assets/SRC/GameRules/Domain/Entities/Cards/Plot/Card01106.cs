using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01106 : CardPlot
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(_chaptersProvider.CurrentScene.DangerDiscardZone.Cards, _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true)
                .Start();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone).Start();

            while (!_chaptersProvider.CurrentScene.DangerDeckZone.TopCard.Tags.Contains(Tag.Ghoul))
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone.TopCard).Start();

            await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(_investigatorProvider.Leader).Start();
        }
    }
}
