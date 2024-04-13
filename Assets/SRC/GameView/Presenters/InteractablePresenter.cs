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
            if (gamAction is InteractableGameAction interactableGameAction)
            {
                mustShowInCenter = interactableGameAction.IsManadatary;
                return await Initial(interactableGameAction);
            }
            return default;
        }

        /*******************************************************************/
        public async Task<Effect> Initial(InteractableGameAction interactableGameAction)
        {
            if (interactableGameAction.IsUniqueCard && mustShowInCenter)
            {
                return await InteractWithMultiEfefct(interactableGameAction, _cardViewManager.GetCardView(interactableGameAction.UniqueCard));
            }
            else if (interactableGameAction.IsManadatary && mustShowInCenter)
            {
                return await InteractWithShowCenter(interactableGameAction);
            }
            else
            {
                return await InteractSingle(interactableGameAction);
            }
        }

        private async Task<Effect> InteractSingle(InteractableGameAction interactableGameAction)
        {
            await DotweenExtension.WaitForMoveToZoneComplete();
            await _showSelectorComponent.ReturnPlayableWithActivation();
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
                return await InteractWithMultiEfefct(interactableGameAction, playableChoose as CardView);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<Effect> InteractWithShowCenter(InteractableGameAction interactableGameAction)
        {
            await DotweenExtension.WaitForMoveToZoneComplete();
            await _showSelectorComponent.ShowPlayables();
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
                return await InteractWithMultiEfefct(interactableGameAction, playableChoose as CardView);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<Effect> InteractWithMultiEfefct(InteractableGameAction interactableGameAction, CardView multiEffectCardView)
        {
            Effect effect = await _multiEffectHandler.ShowMultiEffects(multiEffectCardView);
            if (effect != null)
            {
                return effect;
            }
            mustShowInCenter = false;
            return await Initial(interactableGameAction);
        }
    }
}
