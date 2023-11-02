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
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensor _cardSensor;

        public Card Card { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Card card)
        {
            Debug.Log("Building...: " + card.Info.Code);
            Card = card;
            SetCommonInfo();
            SetAll();
            SetPicture();
        }

        /*******************************************************************/
        public void ActivateToSelect()
        {
            _glowView.SetGreenGlow();
        }

        public void SetCurrentZoneView(ZoneView zoneView)
        {
            transform.SetParent(zoneView.transform);
            if (zoneView is IZoneBehaviour zoneBahaviour) _cardSensor.SetZoneBahaviour(zoneBahaviour);
        }

        protected abstract void SetAll();

        private void SetCommonInfo()
        {
            name = Card.Info.Code;
            _title.text = Card.Info.Name;
            _description.text = Card.Info.Description;
        }

        private void SetPicture()
        {
            _picture.sprite = _picture.sprite; //TODO
        }
    }
}
