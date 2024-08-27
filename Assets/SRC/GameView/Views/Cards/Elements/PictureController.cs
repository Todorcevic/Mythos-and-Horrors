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
            SetPicture(card.Info.Code);
        }

        /*******************************************************************/
        private async void SetPicture(string address) => await _picture.LoadCardSprite(address);

        public Tween ExaustAnimation()
        {
            return _picture.material.DOColor(ViewValues.EXAUST_COLOR, ViewValues.DEFAULT_FADE);
        }

        public Tween UnexaustAnimation()
        {
            return _picture.material.DOColor(Color.white, ViewValues.DEFAULT_FADE);
        }
    }
}
