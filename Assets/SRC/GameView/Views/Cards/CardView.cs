using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] private CardFrontView _cardFrontView;
        [SerializeField, Required, ChildGameObjectsOnly] private CardBackView _cardBackView;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowView _glowView;

        public Card Card { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Card card)
        {
            Card = card;
            SetAll();
        }

        /*******************************************************************/
        public void ActivateToSelect()
        {

        }

        private void SetAll()
        {
            name = Card.Info.Code;
            _title.text = Card.Info.Name;
            _cardBackView.SetReverse(Card);
        }
    }
}
