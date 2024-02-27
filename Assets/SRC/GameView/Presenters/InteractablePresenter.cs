using MythsAndHorrors.GameRules;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InteractablePresenter : IInteractablePresenter
    {
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;

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
            _showCardHandler.ActiavatePlayables(withMainButton: !interactableGameAction.IsManadatary);

            IPlayable playableChoose = await _clickHandler.WaitingClick();

            await _showCardHandler.DeactivatePlayables();
            await _showSelectorComponent.CheckIfIsInSelectorAndReturnPlayables(exceptThisPlayable: playableChoose);


            if (playableChoose.IsMultiEffect)
            {
                return await _multiEffectHandler.ShowMultiEffects(playableChoose as CardView) ?? await Interact(interactableGameAction);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }
    }
}
