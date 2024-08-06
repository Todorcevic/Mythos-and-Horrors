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
        private InteractableText _interactableText;

        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly CardViewsManager _cardViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TextsManager _textsManager;

        /*******************************************************************/
        async Task<BaseEffect> IInteractablePresenter.SelectWith(GameAction gamAction)
        {
            if (gamAction is not InteractableGameAction interactableGameAction) return default;
            _interactableGameAction = interactableGameAction;

            if (!_textsManager.InteractableTexts.TryGetValue(_interactableGameAction.Code, out _interactableText))
            {
                _interactableText = new InteractableText(_interactableGameAction.Code, _interactableGameAction.MustShowInCenter);
            }

            _mainButtonComponent.SetEffect(_interactableGameAction.MainButtonEffect);

            if (_interactableGameAction is IPersonalInteractable personalInteractable)
            {
                await _swapInvestigatorHandler.Select(personalInteractable.ActiveInvestigator).AsyncWaitForCompletion();
            }

            _mustShowInCenter = _interactableText.MustShowInCenter;
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
                await _showSelectorComponent.ShowCards(cardsToShow, GetRealTitle());
                return await Interact();
            }
            else
            {
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
            _mustShowInCenter = _interactableText.MustShowInCenter;
            return await _multiEffectHandler.ShowMultiEffects(multiEffectCardView, _interactableGameAction.Code)
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

        private string GetRealTitle()
        {
            if (_interactableGameAction is CheckMaxHandSizeGameAction checkMaxHandSize)
            {
                int cardsLeft = checkMaxHandSize.ActiveInvestigator.HandSize - checkMaxHandSize.ActiveInvestigator.MaxHandSize.Value;
                return _interactableText.Title.ParseViewWith(checkMaxHandSize.ActiveInvestigator.MaxHandSize.Value.ToString(), cardsLeft.ToString());
            }

            return _interactableText.Title;
        }
    }
}
