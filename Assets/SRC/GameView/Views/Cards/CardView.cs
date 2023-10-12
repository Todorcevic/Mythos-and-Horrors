using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public abstract class CardView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowView _glowView;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;

        public Card Card { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Card card)
        {
            Card = card;
            Debug.Log("Doing...: " + Card.Info.Code);

            SetAll();
            SetInfo();
        }

        /*******************************************************************/
        public void ActivateToSelect()
        {
            _glowView.SetGreenGlow();
        }

        protected abstract void SetAll();

        private void SetInfo()
        {
            _title.text = Card.Info.Name;
            _description.text = Card.Info.Description;
        }
    }
}
