using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class SanityCounterController : CounterController, IStatable
    {
        private IFearable _fearable;

        public Stat Stat { get; private set; }
        public Transform StatTransform => LastShowed.transform;

        /*******************************************************************/
        public void Init(IFearable fearable)
        {
            _fearable = fearable;
            Stat = _fearable.FearRecived;

            EnableThisAmount(_fearable.Sanity.Value);
            ShowThisAmount(_fearable.SanityLeft);
        }

        public Tween UpdateAnimation()
        {
            ShowThisAmount(_fearable.SanityLeft);
            return DOTween.Sequence();
        }
    }
}
