using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class CardChallengeController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _card;

        /*******************************************************************/
        public async Task SetCard(Card card)
        {
            await _card.LoadCardSprite(card.Info.Code);
        }

        /*******************************************************************/
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {

        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {

        }
    }
}

