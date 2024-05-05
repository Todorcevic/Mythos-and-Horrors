using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatView : MonoBehaviour, IStatable
    {
        private const float GLOW_INTENSITY = 0.4f;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _holder;
        [Inject] private readonly StatableManager _statableManager;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;
        public bool IsActive => Stat != null;

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
        Tween IStatable.UpdateValue() => DOTween.Sequence()
                  .Append(_value.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InCubic))
                  .InsertCallback(ViewValues.FAST_TIME_ANIMATION, () => _value.text = Stat.Value.ToString())
                  .Append(_value.transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION * 0.75f).SetEase(Ease.OutBack, 3f));


        /*******************************************************************/
        private ViewState state;

        public void Active()
        {
            if (state == ViewState.Active) return;
            _value.fontMaterial.SetColor("_GlowColor", ViewValues.ACTIVE_COLOR);
            _value.fontSharedMaterial.SetFloat("_GlowPower", GLOW_INTENSITY);
            state = ViewState.Active;
        }

        public void Deactive()
        {
            if (state == ViewState.Deactive) return;
            _value.fontMaterial.SetColor("_GlowColor", ViewValues.DEACTIVE_COLOR);
            _value.fontSharedMaterial.SetFloat("_GlowPower", GLOW_INTENSITY);
            state = ViewState.Deactive;
        }

        public void Default()
        {
            if (state == ViewState.Default) return;
            _value.fontSharedMaterial.SetFloat("_GlowPower", 0f);
            state = ViewState.Default;
        }
    }
}
