using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class PhaseView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required] public Phase _phase;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _name;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _description;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _descriptionBackground;

        public Phase Phase => _phase;

        /*******************************************************************/
        public Tween Show() => DOTween.Sequence().OnStart(() => gameObject.SetActive(true))
                .Append(transform.DOScale(Vector3.one, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.OutBounce, 1.1f));

        public Tween Hide() => transform.DOScale(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutExpo)
            .OnComplete(() => gameObject.SetActive(false));

        public Sequence ChangeText(string description) => ChangeText(_name.text, description)
                .Join(ShowDescription())
                .AppendInterval(ViewValues.SLOW_TIME_ANIMATION * 4)
                .Append(HideDescription());

        public Sequence ChangeText(string name, string description)
        {
            Sequence namingSequence = DOTween.Sequence();
            if (_name.text != name)
            {
                namingSequence.Join(DOTween.Sequence().Join(_name.DOFade(0, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f).OnComplete(() => _name.text = name))
                .Append(_name.DOFade(1, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f)));
            }
            if (_description.text != description)
            {
                namingSequence.Join(DOTween.Sequence().Join(_description.DOFade(0, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f).OnComplete(() => _description.text = description))
                .Append(_description.DOFade(1, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f)));

            }
            return namingSequence;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.1f, ViewValues.FAST_TIME_ANIMATION);
            ShowDescription();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION);
            HideDescription();
        }

        private Tween ShowDescription() => _descriptionBackground.transform.DOLocalMoveX(0, ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.OutBounce);

        private Tween HideDescription() => _descriptionBackground.transform.DOLocalMoveX(-200, ViewValues.DEFAULT_TIME_ANIMATION);
    }
}