using MythsAndHorrors.GameRules;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IInteractablePresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivateCardViewsHandler _showCardHandler;
        [Inject] private readonly ClickHandler<CardView> _clickHandler;

        /*******************************************************************/
        async Task<Effect> IInteractablePresenter.SelectWith(GameAction gamAction)
        {
            if (gamAction is InteractableGameAction interactableGameAction)
            {
                return await Interact(interactableGameAction);
            }
            return default;
        }

        /*******************************************************************/
        private async Task<Effect> Interact(InteractableGameAction interactableGameAction)
        {
            await DotweenExtension.WaitForAllTweensToComplete();
            if (interactableGameAction.IsManadatary) await _showSelectorComponent.ShowPlayables();
            _showCardHandler.ActiavateCardViewsPlayables(_cardViewsManager.GetCardViews(interactableGameAction.ActivableCards), withMainButton: !interactableGameAction.IsManadatary);

            CardView cardViewChoose = await _clickHandler.WaitingClick();

            await _showCardHandler.DeactivateCardViewsPlayables(_cardViewsManager.GetCardViews(interactableGameAction.ActivableCards));
            await _showSelectorComponent.CheckIfIsInSelectorAndReturnPlayables(exceptThis: cardViewChoose);
            return cardViewChoose?.Card.HasMultiEffect ?? false ?
              await _multiEffectHandler.ShowMultiEffects(cardViewChoose) ?? await Interact(interactableGameAction) :
              cardViewChoose?.Card.PlayableEffects.FirstOrDefault();
        }
    }
}
