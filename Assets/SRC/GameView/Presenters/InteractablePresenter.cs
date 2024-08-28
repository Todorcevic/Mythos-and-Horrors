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
                await _showSelectorComponent.ShowCards(cardsToShow, _interactableGameAction.Description);
                return await Interact();
            }
            else
            {
                _phaseComponent.ShowText(_interactableGameAction.Description).SetNotWaitable();
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

            if (playableChoose.IsMultiEffect)
            {
                return await InteractWithMultiEfefct((CardView)playableChoose);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<BaseEffect> InteractWithMultiEfefct(CardView multiEffectCardView)
        {
            _mustShowInCenter = _interactableGameAction.MustShowInCenter;
            return await _multiEffectHandler.ShowMultiEffects(multiEffectCardView, _interactableGameAction.Description)
                ?? await Initial();
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
