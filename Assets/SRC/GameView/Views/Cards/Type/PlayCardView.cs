using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PlayCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private List<FactionInvestigatorSO> _factions;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public void SetInvestigator(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override void SetSpecific()
        {
            FactionInvestigatorSO currentFaction = SetCurrent(Card.Info.Faction);
            _template.sprite = currentFaction._templatePlayCard;

            FactionInvestigatorSO SetCurrent(Faction faction) =>
              _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
              _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);
        }
    }
}
