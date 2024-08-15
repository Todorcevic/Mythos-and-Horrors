using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{

    public class PhaseComponent : MonoBehaviour
    {
        private const float OFFSET = -140f;
        private Investigator currentInvestigator;
        private PhaseView _currentPhaseView;
        [SerializeField, Required, ChildGameObjectsOnly] private List<PhaseView> _phaseViews;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _avatarImage;
        [SerializeField, Required, ChildGameObjectsOnly] private TurnController _turnController;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public Tween ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            PhaseView newPhase = _phaseViews.First(phaseView => phaseView.Phase == phaseGameAction.MainPhase);
            if (newPhase == _currentPhaseView)
            {
                return newPhase.ChangeText(phaseGameAction.Name, phaseGameAction.Description).Join(SetAvatar(phaseGameAction.ActiveInvestigator));
            }
            return DOTween.Sequence()
                .Append(_currentPhaseView?.Hide() ?? DOTween.Sequence())
                .Append(newPhase.Show())
                .Join(SetAvatar(phaseGameAction.ActiveInvestigator))
                .Join(newPhase.ChangeText(phaseGameAction.Name, phaseGameAction.Description))
                .OnComplete(() => _currentPhaseView = newPhase);
        }

        public Tween ShowText(string information) => _currentPhaseView?.ChangeText(information) ?? DOTween.Sequence();

        private Tween SetAvatar(Investigator investigatorTurn)
        {
            if (investigatorTurn == null) return HideAvatar();
            if (currentInvestigator != investigatorTurn) return DOTween.Sequence()
                    .Join(HideAvatar().OnComplete(Set))
                    .Append(ShowAvatar());
            return DOTween.Sequence();

            void Set()
            {
                _turnController.Init(investigatorTurn.CurrentTurns, investigatorTurn.MaxTurns);
                _avatarImage.sprite = _avatarViewsManager.GetByCode(investigatorTurn.Code)?.Image;
                currentInvestigator = investigatorTurn;
            }
        }

        private Tween ShowAvatar() => _avatarImage.transform.DOLocalMoveY(OFFSET, ViewValues.DEFAULT_TIME_ANIMATION)
            .OnStart(() => _avatarImage.gameObject.SetActive(true)).SetEase(Ease.OutBounce, 1.1f);

        private Tween HideAvatar() => _avatarImage.transform.DOLocalMoveY(0, ViewValues.DEFAULT_TIME_ANIMATION)
            .OnComplete(() => { currentInvestigator = null; _avatarImage.gameObject.SetActive(false); });
    }
}