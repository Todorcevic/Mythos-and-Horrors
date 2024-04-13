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
        public override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.DangerDiscardZone.Cards,
                _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(_chaptersProvider.CurrentScene.DangerDeckZone));

            while (_chaptersProvider.CurrentScene.DangerDeckZone.TopCard is not IGhoul)
                await _gameActionsProvider.Create(new DiscardGameAction(_chaptersProvider.CurrentScene.DangerDeckZone.TopCard));

            await _gameActionsProvider.Create(new DrawDangerGameAction(_investigatorProvider.Leader));
        }
    }
}
