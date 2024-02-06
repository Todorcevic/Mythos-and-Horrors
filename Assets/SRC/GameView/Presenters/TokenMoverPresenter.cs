using Zenject;
using MythsAndHorrors.GameRules;
using DG.Tweening;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameView
{
    public class TokenMoverPresenter : IPresenter
    {
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;
        [Inject] private readonly StatableManager _statableManager;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is GainHintGameAction gainHintsGameAction)
                await GainHints(gainHintsGameAction).AsyncWaitForCompletion();
            else if (gamAction is PayHintGameAction payHintsGameAction)
                await PayHints(payHintsGameAction).AsyncWaitForCompletion();
            else if (gamAction is GainResourceGameAction gainResourceGameAction)
                await GainResource(gainResourceGameAction).AsyncWaitForCompletion();
            else if (gamAction is PayResourceGameAction payResourceGameAction)
                await PayResource(payResourceGameAction).AsyncWaitForCompletion();
        }

        /*******************************************************************/
        private Tween GainHints(GainHintGameAction gainHintGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(gainHintGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(gainHintGameAction.Investigator).HintsTokenController
                       .Gain(gainHintGameAction.Amount, _statableManager.Get(gainHintGameAction.FromStat).StatTransform));
        }

        private Tween PayHints(PayHintGameAction payHintGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(payHintGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(payHintGameAction.Investigator).HintsTokenController
                       .Pay(payHintGameAction.Amount, _statableManager.Get(payHintGameAction.ToStat).StatTransform));
        }

        private Tween GainResource(GainResourceGameAction gainResourceGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(gainResourceGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(gainResourceGameAction.Investigator).ResourcesTokenController
                       .Gain(gainResourceGameAction.Amount, _statableManager.Get(gainResourceGameAction.FromStat).StatTransform));
        }

        private Tween PayResource(PayResourceGameAction payResourceGameAction)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorPresenter.Select(payResourceGameAction.Investigator))
                .Append(_areaInvestigatorViewsManager.Get(payResourceGameAction.Investigator).ResourcesTokenController
                       .Pay(payResourceGameAction.Amount, _statableManager.Get(payResourceGameAction.ToStat).StatTransform));
        }
    }
}
