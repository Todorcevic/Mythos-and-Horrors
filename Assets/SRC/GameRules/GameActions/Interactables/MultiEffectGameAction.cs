using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MultiEffectSelectionGameAction : InteractableGameAction
    {
        [Inject] private readonly IInteractableEffectSelectionHandler _interactableEffectSelection;

        public List<Effect> Effects { get; private set; }
        public Effect EffectSelected { get; private set; }

        /*******************************************************************/
        public async Task<Effect> Run(List<Effect> effects)
        {
            Effects = effects;
            await Start();
            return EffectSelected;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            EffectSelected = await _interactableEffectSelection.Interact(this);
        }
    }
}

