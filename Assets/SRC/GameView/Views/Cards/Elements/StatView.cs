using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StatView : MonoBehaviour, IStatable
    {
        [Inject] private readonly StatableManager _statsViewsManager;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _holder;

        public Stat Stat { get; private set; }
        public CardView CardView => GetComponentInParent<CardView>();
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void SetStat(Stat stat, Sprite holderImage = null)
        {
            gameObject.SetActive(true);
            Stat = stat;
            _value.text = stat.Value.ToString();
            _holder.sprite = holderImage ?? _holder.sprite;
            _statsViewsManager.Add(this);
        }

        /*******************************************************************/
        public Tween UpdateValue(int value)
        {
            return DOTween.Sequence().AppendCallback(() => _value.text = value.ToString());
        }
    }
}
