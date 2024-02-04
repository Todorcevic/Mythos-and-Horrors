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
