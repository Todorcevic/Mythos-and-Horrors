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
        private bool mustShowInCenter;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly CardViewsManager _cardViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        /*******************************************************************/
        async Task<Effect> IInteractablePresenter.SelectWith(GameAction gamAction)
        {
            if (gamAction is not InteractableGameAction interactableGameAction) return default;
            await _swapInvestigatorHandler.Select(interactableGameAction.ActiveInvestigator).AsyncWaitForCompletion();
            mustShowInCenter = interactableGameAction.MustShowInCenter;
            return await Initial(interactableGameAction, interactableGameAction.Description);
        }

        /*******************************************************************/
        public async Task<Effect> Initial(InteractableGameAction interactableGameAction, string title)
        {
            await DotweenExtension.WaitForMoveToZoneComplete();

            if (interactableGameAction.IsMultiEffect && mustShowInCenter)
            {
                return await InteractWithMultiEfefct(interactableGameAction, _cardViewManager.GetCardView(interactableGameAction.UniqueCard));
            }
            else if (mustShowInCenter)
            {
                List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
                await _showSelectorComponent.ShowCards(cardsToShow, title);
                return await Interact(interactableGameAction);
            }
            else
            {
                /*********************/
                List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
                Sequence returnSequence = DOTween.Sequence();
                cardsToShow.ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone), Ease.InSine)));
                /*********************/

                await _showSelectorComponent.ShowDown(returnSequence, withActivation: true);
                return await Interact(interactableGameAction);
            }
        }

        private async Task<Effect> Interact(InteractableGameAction interactableGameAction)
        {
            _showCardHandler.ActiavatePlayables();

            IPlayable playableChoose = await _clickHandler.WaitingClick();

            await _showCardHandler.DeactivatePlayables();
            if (playableChoose is ShowCardsInCenterButton)
            {
                mustShowInCenter = !mustShowInCenter;
                return await Initial(interactableGameAction, interactableGameAction.Description);
            }

            /*********************/
            List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
            CardView cardViewSelected = playableChoose as CardView;
            Sequence returnSequence = DOTween.Sequence();
            cardsToShow.Except(new CardView[] { cardViewSelected })
                .ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone), Ease.InSine)));

            if (cardViewSelected != null)
            {
                returnSequence.Append(cardViewSelected.MoveToZone(_zoneViewsManager.CenterShowZone, Ease.InSine));
                if (!playableChoose?.IsMultiEffect ?? false)
                    returnSequence.Append(cardViewSelected.MoveToZone(_zoneViewsManager.Get(cardViewSelected.Card.CurrentZone), Ease.InSine));
            }
            /*********************/

            await _showSelectorComponent.ShowDown(returnSequence, withActivation: false);

            if (playableChoose.IsMultiEffect)
            {
                return await InteractWithMultiEfefct(interactableGameAction, (CardView)playableChoose);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<Effect> InteractWithMultiEfefct(InteractableGameAction interactableGameAction, CardView multiEffectCardView)
        {
            mustShowInCenter = interactableGameAction.MustShowInCenter;
            return await _multiEffectHandler.ShowMultiEffects(multiEffectCardView, interactableGameAction.Description)
                ?? await Initial(interactableGameAction, interactableGameAction.Description);
        }
    }
}
