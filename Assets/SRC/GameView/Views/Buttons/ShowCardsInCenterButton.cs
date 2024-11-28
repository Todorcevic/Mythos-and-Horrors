using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowCardsInCenterButton : UIButton, IPlayable
    {
        private readonly BaseEffect showCardEffect = new(null, null, PlayActionType.None, null, new Localization(string.Empty));
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;

        IEnumerable<BaseEffect> IPlayable.EffectsSelected => new[] { showCardEffect };
        public bool HasThisEffect(BaseEffect baseEffect) => baseEffect == showCardEffect;

        /*******************************************************************/
        public void ActivateToClick()
        {
            _isPlayable = true;
            _icon.DOFade(1f, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
        }

        public void DeactivateToClick()
        {
            _isPlayable = false;
            _icon.DOFade(0.5f, ViewValues.FAST_TIME_ANIMATION).SetNotWaitable();
        }

        protected override void Clicked()
        {
            _interactionHandler.Clicked(this);
        }
    }
}
