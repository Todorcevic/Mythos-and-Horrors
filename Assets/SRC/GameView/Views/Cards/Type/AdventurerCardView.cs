using MythsAndHorrors.EditMode;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
{
    public class AdventurerCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private FactionAdventurerSO _versatile;
        [SerializeField, Required, AssetsOnly] private FactionAdventurerSO _cunning;
        [SerializeField, Required, AssetsOnly] private FactionAdventurerSO _brave;
        [SerializeField, Required, AssetsOnly] private FactionAdventurerSO _scholarly;
        [SerializeField, Required, AssetsOnly] private FactionAdventurerSO _esoteric;
        [SerializeField, Required, AssetsOnly] private FactionAdventurerSO _neutral;

        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SpriteRenderer> _statsRenderer;

        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _agility;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _intelligence;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _power;

        /*******************************************************************/
        protected override void SetAll()
        {
            SetRenderer();
            SetInfo();
        }

        /*******************************************************************/
        private void SetRenderer()
        {
            FactionAdventurerSO currentFaction = SetCurrent(Card.Info.Faction);
            _template.sprite = currentFaction._templateFront;
            _badge.sprite = currentFaction._badget;
            _statsRenderer.ForEach(spriteRenderer => spriteRenderer.sprite = currentFaction._stats);
        }

        private FactionAdventurerSO SetCurrent(Faction faction)
        {
            return faction switch
            {
                Faction.Cunning => _cunning,
                Faction.Versatile => _versatile,
                Faction.Brave => _brave,
                Faction.Esoteric => _esoteric,
                Faction.Scholarly => _scholarly,
                _ => _neutral
            };
        }

        private void SetInfo()
        {
            _health.text = Card.Info.Health.ToString();
            _sanity.text = Card.Info.Sanity.ToString();
            _strength.text = Card.Info.Strength.ToString();
            _agility.text = Card.Info.Agility.ToString();
            _intelligence.text = Card.Info.Intelligence.ToString();
            _power.text = Card.Info.Power.ToString();
        }
    }
}
