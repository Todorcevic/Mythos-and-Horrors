using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class DrawCardPresenter : IPresenter<DrawGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TextsManager _textsProvider;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ShowCardsInCenterButton _showCardsInCenterButton;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(DrawGameAction drawGameAction)
        {
            _mainButtonComponent.SetEffect(new BaseEffect(null, null, PlayActionType.None, null, new Localization("MainButton_Continue")));

            await DOTween.Sequence()
                 .Join(_moveCardHandler.MoveCardtoCenter(drawGameAction.CardDrawed))
                 .Join(_showSelectorComponent.MainButtonWaitingToContinueShowUp()).AsyncWaitForCompletion();

            Tween idle = _cardViewsManager.GetCardView(drawGameAction.CardDrawed).Idle();
            await _clickHandler.WaitingClick();
            idle.Kill();

            _showSelectorComponent.MainButtonWaitingToContinueHideUp();
        }

        private async Task PauseToContinue()
        {
            _mainButtonComponent.SetEffect(new BaseEffect(null, null, PlayActionType.None, null, new Localization("MainButton_Continue")));
            await _clickHandler.WaitingClick();
        }
    }
}
