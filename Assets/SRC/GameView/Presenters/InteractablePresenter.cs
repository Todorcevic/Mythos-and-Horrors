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
        [Inject] private readonly PhaseComponent _phaseComponent;
        [Inject] private readonly TextsManager _textsManager;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        async Task<BaseEffect> IInteractablePresenter.SelectWith(InteractableGameAction interactableGameAction)
        {
            _interactableGameAction = interactableGameAction;
            _mainButtonComponent.SetEffect(_interactableGameAction.MainButtonEffect);

            if (!_textsManager.InteractableTexts.TryGetValue(_interactableGameAction.Code, out _interactableText))
            {
                _interactableText = new InteractableText(_interactableGameAction.Code, _interactableGameAction.MustShowInCenter);
            }

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
                _phaseComponent.ShowText(GetRealTitle()).SetNotWaitable();
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
            switch (_interactableGameAction.Code)
            {
                case "CheckMaxHandSize":
                    CheckMaxHandSizeGameAction checkMaxHandSizeGameAction = (CheckMaxHandSizeGameAction)_interactableGameAction;
                    int cardsLeft = checkMaxHandSizeGameAction.ActiveInvestigator.HandSize - checkMaxHandSizeGameAction.ActiveInvestigator.MaxHandSize.Value;
                    return _interactableText.Title.ParseViewWith(checkMaxHandSizeGameAction.ActiveInvestigator.MaxHandSize.Value.ToString(), cardsLeft.ToString());

                case "CheckSlots":
                    CheckSlotsGameAction checkSlotsGameAction = (CheckSlotsGameAction)_interactableGameAction;
                    string slotsToRemove = string.Empty;
                    checkSlotsGameAction.ActiveInvestigator.GetAllSlotsExeded().ForEach(slot => slotsToRemove += slot.ToString() + "-");
                    slotsToRemove = slotsToRemove.Remove(slotsToRemove.Length - 1);
                    return _interactableText.Title.ParseViewWith(slotsToRemove);

                case "OneInvestigatorTurn":
                    OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction = (OneInvestigatorTurnGameAction)_interactableGameAction;
                    CardInvestigator investigatorCard = oneInvestigatorTurnGameAction.ActiveInvestigator.InvestigatorCard;
                    return _interactableText.Title.ParseViewWith(investigatorCard.Info.Name, investigatorCard.CurrentTurns.Value.ToString());

                case "ShareDamageAndFear":
                    ShareDamageAndFearGameAction shareDamageAndFearGameAction = (ShareDamageAndFearGameAction)_interactableGameAction;
                    return _interactableText.Title.ParseViewWith(shareDamageAndFearGameAction.AmountDamage.ToString(), shareDamageAndFearGameAction.AmountFear.ToString());

                case "Card01158":
                    Card01158 card01158 = _cardsProvider.GetCard<Card01158>();
                    return _interactableText.Title.ParseViewWith((card01158.ChoiseRemaining.Value + 1).ToString());

                case "Card01138":
                    Card01138 card01138 = _cardsProvider.GetCard<Card01138>();
                    return _interactableText.Title.ParseViewWith((card01138.DiscardRemaining.Value + 1).ToString());
            }

            return _interactableText.Title;
        }
    }
}
