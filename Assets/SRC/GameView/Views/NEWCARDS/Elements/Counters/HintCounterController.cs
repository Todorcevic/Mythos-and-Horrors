using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{

    public class HintCounterController : CounterController, IStatable
    {
        private CardPlace _hintable;

        public Stat Stat { get; private set; }
        public Transform StatTransform => transform;

        /*******************************************************************/
        public void Init(CardPlace hintable)
        {
            _hintable = hintable;
            Stat = _hintable.Hints;

            EnableThisAmount(_hintable.Hints.Value);
            ShowThisAmount(_hintable.Hints.Value);
            gameObject.SetActive(true);
        }

        public Tween UpdateValue()
        {
            if (_hintable.Hints.Value > AmountEnable)
                EnableThisAmount(_hintable.Hints.Value);

            ShowThisAmount(_hintable.Hints.Value);
            return DOTween.Sequence();
        }
    }
}
