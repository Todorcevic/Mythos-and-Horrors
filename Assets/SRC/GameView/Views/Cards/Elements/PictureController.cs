using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class PictureController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;

        public Sprite Picture => _picture.sprite;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IRevealable revelable && revelable.Revealed.IsActive) _ = _picture.LoadRevealedCardSprite(card.Info.Code);
            else _ = _picture.LoadCardSprite(card.Info.Code);
        }

        public Tween UpdateImageAnimation(Card card) => DOTween.Sequence().Join(_picture.DOColor(Color.black, ViewValues.VERYFAST_TIME_ANIMATION))
                 .InsertCallback(ViewValues.VERYFAST_TIME_ANIMATION, () => Init(card))
                 .Append(_picture.DOColor(Color.white, ViewValues.VERYFAST_TIME_ANIMATION));

        /*******************************************************************/
        public Tween ExaustAnimation()
        {
            return _picture.material.DOColor(ViewValues.EXAUST_COLOR, ViewValues.MID_TIME_ANIMATION);
        }

        public Tween UnexaustAnimation()
        {
            return _picture.material.DOColor(Color.white, ViewValues.MID_TIME_ANIMATION);
        }
    }
}
