using Zenject;
using MythsAndHorrors.GameRules;
using DG.Tweening;

namespace MythsAndHorrors.GameView
{
    public class TokenMoverPresenter : IPresenter
    {
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly StatableManager _statableManager;

        /*******************************************************************/
        public Tween GainHints(GainHintGameAction gainHintGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(gainHintGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(gainHintGameAction.Investigator).HintsTokenController
                       .Gain(gainHintGameAction.Amount, _statableManager.Get(gainHintGameAction.FromStat).StatTransform));
        }

        public Tween PayHints(PayHintGameAction payHintGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(payHintGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(payHintGameAction.Investigator).HintsTokenController
                       .Pay(payHintGameAction.Amount, _statableManager.Get(payHintGameAction.ToStat).StatTransform));
        }

        public Tween GainResource(GainResourceGameAction gainResourceGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(gainResourceGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(gainResourceGameAction.Investigator).ResourcesTokenController
                       .Gain(gainResourceGameAction.Amount, _statableManager.Get(gainResourceGameAction.FromStat).StatTransform));
        }

        public Tween PayResource(PayResourceGameAction payResourceGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(payResourceGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(payResourceGameAction.Investigator).ResourcesTokenController
                       .Pay(payResourceGameAction.Amount, _statableManager.Get(payResourceGameAction.ToStat).StatTransform));

        }
    }
}
