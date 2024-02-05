using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StatView : MonoBehaviour, IStatableView
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _holder;
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
        public Tween UpdateValue()
        {
            return DOTween.Sequence()
                 .Append(_value.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InCubic))
                 .InsertCallback(ViewValues.FAST_TIME_ANIMATION, () => _value.text = Stat.Value.ToString())
                 .Append(_value.transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION * 0.75f).SetEase(Ease.OutBack, 3f));
        }
    }
}
