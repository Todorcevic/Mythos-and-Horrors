using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PhaseComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<PhaseView> _phaseViews;
        [SerializeField, Required, ChildGameObjectsOnly] private PhaseView _currentPhaseView;

        /*******************************************************************/
        public Tween ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            PhaseView newPhase = _phaseViews.Find(phaseView => phaseView.Phase == phaseGameAction.MainPhase);
            if (newPhase == null) return DOTween.Sequence();
            if (newPhase == _currentPhaseView) return newPhase.ChangeText(phaseGameAction.Name, phaseGameAction.Description);
            return DOTween.Sequence().Append(_currentPhaseView.Hide())
                .Append(newPhase.Show())
                .Join(newPhase.ChangeText(phaseGameAction.Name, phaseGameAction.Description))
                .OnComplete(() => _currentPhaseView = newPhase);
        }
    }
}