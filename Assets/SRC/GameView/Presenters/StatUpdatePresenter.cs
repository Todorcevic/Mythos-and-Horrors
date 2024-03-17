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
        private Dictionary<IStatableView, bool> statablesUpdated;
        [Inject] private readonly StatableManager _statsViewsManager;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TokenMoverHandler _tokenMoverHandler;

        /*******************************************************************/
        async Task IPresenter<StatGameAction>.PlayAnimationWith(StatGameAction updateStatGameAction)
        {
            statablesUpdated = _statsViewsManager.GetAll(updateStatGameAction.AllStats).ToDictionary(statView => statView, _ => false);
            await SpecialAnimations(updateStatGameAction);
            Update(_statsViewsManager.GetAll(updateStatGameAction.AllStats));
        }

        /*******************************************************************/
        private async Task SpecialAnimations(StatGameAction updateStatGameAction)
        {
            if (updateStatGameAction.AllStats.Contains(_chaptersProvider.CurrentScene.CurrentPlot?.Eldritch))
            {
                await _moveCardHandler.MoveCardtoCenter(_chaptersProvider.CurrentScene.CurrentPlot).AsyncWaitForCompletion();
                await Update(_statsViewsManager.GetAll(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch)).AsyncWaitForCompletion();
                await _moveCardHandler.ReturnCard(_chaptersProvider.CurrentScene.CurrentPlot).AsyncWaitForCompletion();
            }

            await CheckResources(updateStatGameAction).AsyncWaitForCompletion();
            await CheckHints(updateStatGameAction).AsyncWaitForCompletion();
        }

        private Tween CheckResources(StatGameAction updateStatGameAction)
        {
            Sequence payResourceSequence = DOTween.Sequence();

            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay
                        .Where(investigator => updateStatGameAction.AllStats.Contains(investigator.Resources)))
            {
                int amount = investigator.Resources.Value - investigator.Resources.ValueBeforeUpdate;
                if (amount > 0) payResourceSequence.Append(_tokenMoverHandler.GainResourceAnimation(investigator, amount));
                else if (amount < 0) payResourceSequence.Append(_tokenMoverHandler.PayResourceAnimation(investigator, -1 * amount));
            }

            return payResourceSequence;
        }

        private Tween CheckHints(StatGameAction updateStatGameAction)
        {
            Sequence hintsSequence = DOTween.Sequence();

            if (updateStatGameAction.Parent is GainHintGameAction gainHintGA)
            {
                hintsSequence.Join(Update(_statsViewsManager.GetAll(gainHintGA.FromStat)));
                hintsSequence.Append(_tokenMoverHandler.GainHintsAnimation(gainHintGA.Investigator, gainHintGA.Amount, gainHintGA.FromStat));
            }
            else if (updateStatGameAction.Parent is PayHintGameAction payHintGA)
            {
                hintsSequence.Append(_tokenMoverHandler.PayHintsAnimation(payHintGA.Investigator, payHintGA.Amount, payHintGA.ToStat));
            }

            return hintsSequence;
        }

        private Tween Update(IEnumerable<IStatableView> statView)
        {
            Sequence updateSequence = DOTween.Sequence();
            foreach (IStatableView stat in statView)
            {
                if (statablesUpdated[stat]) continue;
                statablesUpdated[stat] = true;
                updateSequence.Join(stat.UpdateValue());
            }
            return updateSequence;
        }

        /*******************************************************************/
        Task IPresenter<StatGameAction>.UndoAnimationWith(StatGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
