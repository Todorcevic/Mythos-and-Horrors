using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class HintCounterController : CounterController, IStatable
    {
        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(CardPlace hintable)
        {
            Stat = hintable.Hints;
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
