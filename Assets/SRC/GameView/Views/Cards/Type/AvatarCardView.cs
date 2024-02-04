using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class AvatarCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private List<FactionInvestigatorSO> _factions;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _sanity;

        public Investigator Investigator => (Card as CardAvatar).Investigator;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            FactionInvestigatorSO currentFaction = SetCurrent(Card.Info.Faction);
            _template.sprite = currentFaction._templatePlayCard;
            _title.color = currentFaction._color;

            FactionInvestigatorSO SetCurrent(Faction faction) =>
              _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
              _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);

            SetStats();
        }

        /*******************************************************************/
        private void SetStats()
        {
            _health.SetStat(Investigator.InvestigatorCard.Health);
            _sanity.SetStat(Investigator.InvestigatorCard.Sanity);
        }
    }
}
