using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class InvestigatorCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private List<FactionInvestigatorSO> _factions;
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
        protected override void SetSpecific()
        {
            SetRenderer();
            SetInfo();
        }

        /*******************************************************************/
        private void SetRenderer()
        {
            FactionInvestigatorSO currentFaction = SetCurrent(Card.Info.Faction);
            _template.sprite = currentFaction._templateFront;
            _badge.sprite = currentFaction._badget;
            _statsRenderer.ForEach(spriteRenderer => spriteRenderer.sprite = currentFaction._stats);
        }

        private FactionInvestigatorSO SetCurrent(Faction faction) =>
            _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
            _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);

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
