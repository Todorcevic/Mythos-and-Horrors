using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class StatView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _holder;

        public Stat Stat { get; private set; }

        /*******************************************************************/
        public void SetStat(Stat stat)
        {
            gameObject.SetActive(true);
            Stat = stat;
            _value.text = stat.Value.ToString();
        }

        /*******************************************************************/
        public void UpdateValue(int value)
        {
            _holder.transform.DOMoveY(1, ViewValues.FAST_TIME_ANIMATION);
            _value.transform.DOScale(1.2f, ViewValues.FAST_TIME_ANIMATION);
            _value.text = value.ToString();


            //DOTweenTMPAnimator animator = new DOTweenTMPAnimator(_value);
            //animator.DOPunchCharScale(0, 0.2f, 1.2f);

        }
    }
}
