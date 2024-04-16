using MythosAndHorrors.GameRules;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InteractablePresenter : IInteractablePresenter
    {
        private bool mustShowInCenter;
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly CardViewsManager _cardViewManager;

        /*******************************************************************/
        async Task<Effect> IInteractablePresenter.SelectWith(GameAction gamAction)
        {
            if (gamAction is not InteractableGameAction interactableGameAction) return default;
            mustShowInCenter = interactableGameAction.MustShowInCenter;
            return await Initial(interactableGameAction);
        }

        /*******************************************************************/
        public async Task<Effect> Initial(InteractableGameAction interactableGameAction)
        {
            await DotweenExtension.WaitForMoveToZoneComplete();

            if (interactableGameAction.IsUniqueCard && mustShowInCenter)
            {
                return await InteractWithMultiEfefct(interactableGameAction, _cardViewManager.GetCardView(interactableGameAction.UniqueCard));
            }
            else if (mustShowInCenter)
            {
                await _showSelectorComponent.ShowPlayables();
                return await Interact(interactableGameAction);
            }
            else
            {
                await _showSelectorComponent.ReturnPlayableWithActivation();
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
                return await Initial(interactableGameAction);
            }

            await _showSelectorComponent.CheckIfIsInSelectorAndReturnPlayables(exceptThisPlayable: playableChoose);
            if (playableChoose.IsMultiEffect)
            {
                return await InteractWithMultiEfefct(interactableGameAction, (CardView)playableChoose);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<Effect> InteractWithMultiEfefct(InteractableGameAction interactableGameAction, CardView multiEffectCardView)
        {
            mustShowInCenter = false;
            return await _multiEffectHandler.ShowMultiEffects(multiEffectCardView) ?? await Initial(interactableGameAction);
        }
    }
}
