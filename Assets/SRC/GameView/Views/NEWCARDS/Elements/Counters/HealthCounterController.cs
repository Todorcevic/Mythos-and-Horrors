using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{

    public class HealthCounterController : CounterController, IStatable
    {
        private IDamageable _damageable;

        public Stat Stat { get; private set; }
        public Transform StatTransform => LastShowed.transform;

        /*******************************************************************/
        public void Init(IDamageable damageable)
        {
            _damageable = damageable;
            Stat = _damageable.DamageRecived;

            EnableThisAmount(_damageable.Health.Value);
            ShowThisAmount(_damageable.HealthLeft);
            gameObject.SetActive(true);
        }

        public Tween UpdateAnimation()
        {
            ShowThisAmount(_damageable.HealthLeft);
            return DOTween.Sequence();
        }
    }
}
