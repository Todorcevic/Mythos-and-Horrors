using Zenject;
using MythsAndHorrors.GameRules;
using DG.Tweening;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameView
{
    public class TokenMoverPresenter : IPresenter<GainHintGameAction>, IPresenter<PayHintGameAction>, IPresenter<GainResourceGameAction>, IPresenter<PayResourceGameAction>
    {
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly StatableManager _statableManager;

        /*******************************************************************/
        async Task IPresenter<GainHintGameAction>.PlayAnimationWith(GainHintGameAction gainHintGameAction)
        {
            await DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(gainHintGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(gainHintGameAction.Investigator).HintsTokenController
                    .Gain(gainHintGameAction.Amount, _statableManager.Get(gainHintGameAction.FromStat).StatTransform))
                .AsyncWaitForCompletion();
        }

        async Task IPresenter<PayHintGameAction>.PlayAnimationWith(PayHintGameAction payHintGameAction)
        {
            await DOTween.Sequence()
              .Append(_swapInvestigatorPresenter.Select(payHintGameAction.Investigator))
              .Append(_areaInvestigatorViewsManager.Get(payHintGameAction.Investigator).HintsTokenController
                    .Pay(payHintGameAction.Amount, _statableManager.Get(payHintGameAction.ToStat).StatTransform))
              .AsyncWaitForCompletion();
        }

        async Task IPresenter<GainResourceGameAction>.PlayAnimationWith(GainResourceGameAction gainResourceGameAction)
        {
            await DOTween.Sequence()
               .Append(_swapInvestigatorPresenter.Select(gainResourceGameAction.Investigator))
               .Append(_areaInvestigatorViewsManager.Get(gainResourceGameAction.Investigator).ResourcesTokenController
                   .Gain(gainResourceGameAction.Amount, _statableManager.Get(gainResourceGameAction.FromStat).StatTransform))
               .AsyncWaitForCompletion();
        }

        async Task IPresenter<PayResourceGameAction>.PlayAnimationWith(PayResourceGameAction payResourceGameAction)
        {
            await DOTween.Sequence()
              .Append(_swapInvestigatorPresenter.Select(payResourceGameAction.Investigator))
              .Append(_areaInvestigatorViewsManager.Get(payResourceGameAction.Investigator).ResourcesTokenController
                     .Pay(payResourceGameAction.Amount, _statableManager.Get(payResourceGameAction.ToStat).StatTransform))
              .AsyncWaitForCompletion();
        }
    }
}
