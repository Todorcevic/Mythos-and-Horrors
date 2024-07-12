using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class PictureController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;

        /*******************************************************************/
        public void Init(Card card)
        {
            SetPicture(card.Info.Code);
        }

        /*******************************************************************/
        private async void SetPicture(string address) => await _picture.LoadCardSprite(address);

        public Tween ExaustAnimation() //TODO: Implementar animacion de fade out
        {
            return DOTween.Sequence();
        }

        public Tween UnexaustAnimation() //TODO: Implementar animacion de fade in
        {
            return DOTween.Sequence();
        }
    }
}
