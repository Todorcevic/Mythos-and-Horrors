using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class EldritchCounterController : CounterController, IStatable
    {
        private IEldritchable _eldritchable;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(IEldritchable eldritchable)
        {
            _eldritchable = eldritchable;
            Stat = _eldritchable.Eldritch;
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
            EnableThisAmount(_eldritchable.Eldritch.Value);
            ShowThisAmount(_eldritchable.Eldritch.Value);
        }
    }
}
