using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ResourceCounterController : CounterController, IStatable
    {
        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Stat resourceStat)
        {
            Stat = resourceStat;
            UpdateValue();
        }

        public Tween UpdateAnimation()
        {
            UpdateValue();
            return DOTween.Sequence();
        }

        private void UpdateValue()
        {
            EnableThisAmount(Stat.Value);
            ShowThisAmount(Stat.Value);
            gameObject.SetActive(true);
        }
    }
}
