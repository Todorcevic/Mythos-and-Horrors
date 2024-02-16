using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class PlaceCardView : CardView
    {
        [Title(nameof(PlaceCardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _hints;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _enigma;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _title.text = Card.Info.Name2 ?? Card.Info.Name;
            _description.text = Card.Info.Description2 ?? Card.Info.Flavor2;
            if (Card is CardPlace _place)
            {
                _hints.SetStat(_place.Hints);
                _hints.gameObject.SetActive(false);
                _enigma.SetStat(_place.Enigma);
                _enigma.gameObject.SetActive(false);
            }
        }

        /*******************************************************************/
        public override void UpdateState() { }

        public Tween RevealAnimation()
        {
            return DOTween.Sequence().Append(DOTween.Sequence()
                .Append(transform.DOLocalMoveY(8, ViewValues.DEFAULT_TIME_ANIMATION))
                .Append(transform.DOLocalMoveY(0, ViewValues.DEFAULT_TIME_ANIMATION)))
                .Join(_rotator.Rotate360(ViewValues.DEFAULT_TIME_ANIMATION * 2))
                .InsertCallback(ViewValues.DEFAULT_TIME_ANIMATION, RevealInfo);
        }


        private void RevealInfo()
        {
            _title.text = Card.Info.Name;
            _description.text = Card.Info.Description ?? Card.Info.Flavor;
            _hints.gameObject.SetActive(true);
            _enigma.gameObject.SetActive(true);
        }
    }
}
