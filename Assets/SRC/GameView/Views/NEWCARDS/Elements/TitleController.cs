using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{

    public class TitleController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _name;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _cardType;

        /*******************************************************************/
        public void Init(Card card)
        {
            SetTitle(card.Info.Name);
            SetType(card.Info.CardType);
        }

        /*******************************************************************/
        private void SetTitle(string title) => _name.text = title;

        private void SetType(CardType cardType)
        {
            _cardType.text = cardType switch
            {
                CardType.Investigator => "Investigator",
                CardType.Supply => "Supply",
                CardType.Talent => "Talent",
                CardType.Condition => "Condition",
                CardType.Creature => "Creature",
                CardType.Adversity => "Adversity",
                CardType.Place => "Place",
                CardType.Goal => "Goal",
                CardType.Plot => "Plot",
                CardType.Scene => "Scene",
                _ => string.Empty
            };
        }
    }
}
