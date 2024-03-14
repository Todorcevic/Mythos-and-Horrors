using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatUpdatePresenter : IPresenter<StatGameAction>
    {
        [Inject] private readonly StatableManager _statsViewsManager;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        /*******************************************************************/
        async Task IPresenter<StatGameAction>.PlayAnimationWith(StatGameAction updateStatGameAction)
        {
            await SpecialAnimations(updateStatGameAction);
            UpdateStat(updateStatGameAction);
        }

        /*******************************************************************/
        private Sequence UpdateStat(StatGameAction updateStatGameAction)
        {
            List<IStatableView> statables = _statsViewsManager.GetAll(updateStatGameAction.AllStats);
            Sequence updateStatsSequence = DOTween.Sequence();
            statables.ForEach(statView => updateStatsSequence.Join(statView.UpdateValue()));
            return updateStatsSequence;
        }

        private async Task SpecialAnimations(StatGameAction updateStatGameAction)
        {
            if (updateStatGameAction.AllStats.Contains(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch))
            {
                await _moveCardHandler.MoveCardtoCenter(_chaptersProvider.CurrentScene.CurrentPlot);
                await _statsViewsManager.GetAll(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch).First()
                    .UpdateValue().AsyncWaitForCompletion();
                await _moveCardHandler.ReturnCard(_chaptersProvider.CurrentScene.CurrentPlot);
            }
        }
    }
}
