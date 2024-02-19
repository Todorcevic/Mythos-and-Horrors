using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PhaseView : MonoBehaviour
    {
        [SerializeField, Required] public Phase _phase;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _name;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _description;

        public Phase Phase => _phase;

        /*******************************************************************/
        public Tween Show() => DOTween.Sequence().OnStart(() => gameObject.SetActive(true))
                .Append(transform.DOScale(Vector3.one, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.OutBounce, 1.1f));

        public Tween Hide() => transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutExpo)
            .OnComplete(() => gameObject.SetActive(false));

        public Tween ChangeText(string name, string description) => DOTween.Sequence()
            .Join(_name.DOFade(0, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f))
            .Join(_description.DOFade(0, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f).OnComplete(() => { _name.text = name; _description.text = description; }))
            .Append(_name.DOFade(1, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f))
            .Join(_description.DOFade(1, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f));

    }
}