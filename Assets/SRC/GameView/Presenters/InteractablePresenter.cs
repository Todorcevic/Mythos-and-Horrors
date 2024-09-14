using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InteractablePresenter : IInteractablePresenter
    {
        private bool _mustShowInCenter;
        private InteractableGameAction _interactableGameAction;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly CardViewsManager _cardViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly PhaseComponent _phaseComponent;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly TextsManager _textsProvider;
        [Inject] private readonly ShowCardsInCenterButton _showCardsInCenterButton;

        string InteractableTitle => _textsProvider.GetLocalizableText(_interactableGameAction.InteractableTitle);

        /*******************************************************************/
        async Task<BaseEffect> IInteractablePresenter.SelectWith(InteractableGameAction interactableGameAction)
        {
            _interactableGameAction = interactableGameAction;
            _mainButtonComponent.SetEffect(_interactableGameAction.MainButtonEffect);
            _mustShowInCenter = _interactableGameAction.MustShowInCenter;
            return await Initial();
        }

        /*******************************************************************/
        private async Task<BaseEffect> Initial()
        {
            await DotweenExtension.WaitForMoveToZoneComplete();

            if (_interactableGameAction.IsMultiEffect && _mustShowInCenter)
            {
                return await InteractWithMultiEfefct(_cardViewManager.GetCardView(_interactableGameAction.UniqueCard));
            }
            else if (_mustShowInCenter)
            {
                List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
                await _showSelectorComponent.ShowCards(cardsToShow, InteractableTitle);
                return await Interact();
            }
            else
            {
                _phaseComponent.ShowText(InteractableTitle).SetNotWaitable();
                await CenterShowDown();
                return await Interact();
            }
        }

        private async Task<BaseEffect> Interact()
        {
            _showCardHandler.ActiavatePlayables();

            IPlayable playableChoose = await _clickHandler.WaitingClick();
            await _showCardHandler.DeactivatePlayables();
            if (playableChoose is ShowCardsInCenterButton)
            {
                _mustShowInCenter = !_mustShowInCenter;
                return await Initial();
            }
            await CenterShowDown();

            return playableChoose.IsMultiEffect ? await InteractWithMultiEfefct((CardView)playableChoose) : playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<BaseEffect> InteractWithMultiEfefct(CardView multiEffectCardView)
        {
            _mustShowInCenter = _interactableGameAction.MustShowInCenter;
            BaseEffect baseEffect = await _multiEffectHandler.ShowMultiEffects(multiEffectCardView, InteractableTitle);
            bool isCardPressed = multiEffectCardView.Card.PlayableEffects.Contains(baseEffect);
            bool isShowCardInCenterButton_Pressed = _showCardsInCenterButton.HasThisEffect(baseEffect);
            if (isCardPressed) return baseEffect;
            if (isShowCardInCenterButton_Pressed) _mustShowInCenter = !_mustShowInCenter;
            return await Initial();
        }

        private async Task CenterShowDown()
        {
            if (_showSelectorComponent.IsShowing)
            {
                List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
                Tween returnSequence = _moveCardHandler.MoveCardsToCurrentZones(cardsToShow.Select(cardView => cardView.Card), ease: Ease.OutSine);
                await _showSelectorComponent.ShowDown(returnSequence, withActivation: false);
            }
        }
    }
}
