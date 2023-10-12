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
        [SerializeField, Required, ChildGameObjectsOnly] private CardFrontView _cardFrontView;
        [SerializeField, Required, ChildGameObjectsOnly] private CardBackView _cardBackView;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowView _glowView;
        [SerializeField, Required, ChildGameObjectsOnly] private CardHolderView _cardHolderView;
        [SerializeField, ChildGameObjectsOnly] private SkillPlacerView _skillPlacerView;

        public Card Card { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Card card)
        {
            Card = card;
            Debug.Log("Doing...: " + Card.Info.Code);
            SetAll();
        }

        /*******************************************************************/
        public void ActivateToSelect()
        {
            _glowView.SetGreenGlow();
        }

        private void SetAll()
        {
            name = Card.Info.Code;
            _cardBackView.SetReverse(Card);
            _cardFrontView.SetFront(Card);
            _cardHolderView.SetInfo(Card);
            if (_skillPlacerView != null) _skillPlacerView.SetSkillPlacer(Card);
        }
    }
}
