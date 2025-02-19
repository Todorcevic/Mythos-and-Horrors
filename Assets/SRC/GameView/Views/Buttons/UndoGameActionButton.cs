using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class UndoGameActionButton : UIButton, IPlayable
    {
        [Inject] private readonly ClickHandler _interactionHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private BaseEffect UndoEffect => _gameActionsProvider.CurrentInteractable?.UndoEffect;
        IEnumerable<BaseEffect> IPlayable.EffectsSelected => UndoEffect == null ? Enumerable.Empty<CardEffect>() : new[] { UndoEffect };
        public bool CanBePlayed => UndoEffect != null;

        /*******************************************************************/
        void IPlayable.ActivateToClick()
        {
            _isPlayable = true;
            _icon.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        void IPlayable.DeactivateToClick()
        {
            _isPlayable = false;
            _icon.DOFade(0.5f, ViewValues.FAST_TIME_ANIMATION);
        }

        protected override void Clicked()
        {
            _interactionHandler.Clicked(this);
        }
    }
}
