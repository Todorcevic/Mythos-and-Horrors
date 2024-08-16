using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class DrawCardPresenter : IPresenter<DrawGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ShowCardsInCenterButton _showCardsInCenterButton;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(DrawGameAction drawGameAction)
        {
            CardView cardView = _cardViewsManager.GetCardView(drawGameAction.CardDrawed);
            _mainButtonComponent.SetEffect(new BaseEffect(null, null, PlayActionType.None, null, string.Empty, _textsProvider.GetLocalizableText("MainButton_Continue")));
            _moveCardHandler.MoveCardtoCenter(drawGameAction.CardDrawed);
            await _showSelectorComponent.MainButonShowUp();
            _showCardsInCenterButton.DeactivateToClick();
            Tween idle = cardView.Idle();

            await _clickHandler.WaitingClick();
            idle.Kill();

            await _showSelectorComponent.MainButonHideUp();
        }
    }
}
