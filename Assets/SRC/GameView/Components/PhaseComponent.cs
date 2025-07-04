﻿using DG.Tweening;
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
        [SerializeField, Required, ChildGameObjectsOnly] private ActionController _actionController;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly TextsManager _textsProvider;
        [SerializeField, Required, AssetsOnly] private AudioClip _showAvatar;
        [Inject] private readonly AudioComponent _audioComponent;

        /*******************************************************************/
        public Tween ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            string name = _textsProvider.GetLocalizableText(phaseGameAction.PhaseNameLocalization);
            string description = _textsProvider.GetLocalizableText(phaseGameAction.PhaseDescriptionLocalization);

            PhaseView newPhase = _phaseViews.First(phaseView => phaseView.Phase == phaseGameAction.MainPhase);
            if (newPhase == _currentPhaseView)
            {
                return newPhase.ChangeText(name, description).Join(SetAvatar(phaseGameAction.ActiveInvestigator));
            }
            return DOTween.Sequence()
                .Append(_currentPhaseView?.Hide() ?? DOTween.Sequence())
                .Append(newPhase.Show())
                .Join(SetAvatar(phaseGameAction.ActiveInvestigator))
                .Join(newPhase.ChangeText(name, description)
                .OnComplete(() => _currentPhaseView = newPhase));
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
                _actionController.Init(investigatorTurn.CurrentActions, investigatorTurn.MaxActions);
                _avatarImage.sprite = _avatarViewsManager.GetByCode(investigatorTurn.Code)?.Image;
                currentInvestigator = investigatorTurn;
            }
        }

        private Tween ShowAvatar() => _avatarImage.transform.DOLocalMoveY(OFFSET, ViewValues.FAST_TIME_ANIMATION)
            .OnStart(() => { _avatarImage.gameObject.SetActive(true); _audioComponent.PlayAudio(_showAvatar); });

        private Tween HideAvatar() => _avatarImage.transform.DOLocalMoveY(0, ViewValues.FAST_TIME_ANIMATION)
            .OnComplete(() => { currentInvestigator = null; _avatarImage.gameObject.SetActive(false); });
    }
}