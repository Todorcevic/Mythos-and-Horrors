using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _name;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _cardType;
        [Inject] private readonly TextsManager _textsManager;

        /*******************************************************************/
        public void Init(Card card)
        {
            _name.text = card.CurrentName;
            _cardType.text = _textsManager.GetCardTypeText(card.Info.CardType);
        }
    }
}
