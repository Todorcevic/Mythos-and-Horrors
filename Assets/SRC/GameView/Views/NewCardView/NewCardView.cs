using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public abstract class NewCardView : MonoBehaviour, IPlayable
    {
        private CardEffect _cloneEffect;

        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneCardView _ownZoneCardView;

        public Card Card { get; private set; }
        public ZoneView CurrentZoneView { get; private set; }

        public IEnumerable<BaseEffect> EffectsSelected => _cloneEffect != null ? new[] { _cloneEffect } : Card.PlayableEffects;

        /*******************************************************************/
        public void Init(Card card, ZoneView currentZoneView)
        {
            Card = card;
            SetPicture();
            SetCommon();
            SetSpecific();
            SetInitialCurrentZoneView(currentZoneView);
            Off();

            void SetInitialCurrentZoneView(ZoneView zoneView)
            {
                CurrentZoneView = zoneView;
                transform.SetParent(zoneView.transform);
            }
        }

        protected abstract void SetSpecific();

        /*******************************************************************/

        private async void SetPicture() => await _picture.LoadCardSprite(Card.Info.Code);

        private void SetCommon()
        {
            name = Card.Info.Code;
            _ownZoneCardView.Init(Card.OwnZone);
            _title.text = Card.Info.Name;
            SetDescription(Card.Info.Description ?? Card.Info.Flavor);
        }

        protected void SetDescription(string description)
        {
            _description.text = "";
            if (Card.Info.Tags != null && Card.Info.Tags.Length > 0)
            {
                _description.text = "<size=3><b>";
                Card.Info.Tags.ForEach(tag => _description.text += tag + " - ");
                _description.text = _description.text.Remove(_description.text.Length - 3);
                _description.text += "</b></size>\n";
            }

            _description.text += "\n<voffset=0.25em>" + description + "</voffset>";
        }

        public void On() => gameObject.SetActive(true);

        public void Off() => gameObject.SetActive(false);


        public void ActivateToClick()
        {
            throw new NotImplementedException();
        }

        public void DeactivateToClick()
        {
            throw new NotImplementedException();
        }
    }
}
