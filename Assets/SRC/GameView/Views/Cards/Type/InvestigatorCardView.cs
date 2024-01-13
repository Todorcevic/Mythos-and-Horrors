using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class InvestigatorCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private List<FactionInvestigatorSO> _factions;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SpriteRenderer> _statsRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _agility;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _intelligence;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _power;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            SetRenderer();
            SetStats();
        }

        /*******************************************************************/
        private void SetRenderer()
        {
            FactionInvestigatorSO currentFaction = SetCurrent(Card.Info.Faction);
            _template.sprite = currentFaction._templateFront;
            _badge.sprite = currentFaction._badget;
            _statsRenderer.ForEach(spriteRenderer => spriteRenderer.sprite = currentFaction._stats);

            FactionInvestigatorSO SetCurrent(Faction faction) =>
           _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
           _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);
        }

        private void SetStats()
        {
            if (Card is CardInvestigator _investigator)
            {
                _health.SetStat(_investigator.Health);
                _sanity.SetStat(_investigator.Sanity);
                _strength.SetStat(_investigator.Strength);
                _agility.SetStat(_investigator.Agility);
                _intelligence.SetStat(_investigator.Intelligence);
                _power.SetStat(_investigator.Power);
            }
        }
    }
}
