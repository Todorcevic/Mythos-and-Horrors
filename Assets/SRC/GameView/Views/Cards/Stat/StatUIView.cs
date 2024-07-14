using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatUIView : MonoBehaviour, IStatable
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _holder;
        [Inject] private readonly StatableManager _statableManager;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void SetStat(Stat stat, Sprite holderImage = null)
        {
            gameObject.SetActive(true);
            Stat = stat;
            _statableManager.Add(this);
            _value.text = stat.Value.ToString();
            _holder.sprite = holderImage ?? _holder.sprite;
        }

        /*******************************************************************/
        Tween IStatable.UpdateAnimation() => DOTween.Sequence()
                .Append(_value.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InCubic))
                .InsertCallback(ViewValues.FAST_TIME_ANIMATION, () => _value.text = Stat.Value.ToString())
                .Append(_value.transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION * 0.75f).SetEase(Ease.OutBack, 3f));
    }
}
