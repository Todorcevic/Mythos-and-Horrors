using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class BasicCounterController : CounterController, IStatable
    {
        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(Stat extraStat)
        {
            Stat = extraStat;
            UpdateValue();
            gameObject.SetActive(true);
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

        }
    }
}
