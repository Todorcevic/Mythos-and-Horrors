using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class InvestigatorCardView : CardView
    {
        [Title(nameof(InvestigatorCardView))]
        [SerializeField, Required, AssetsOnly] private List<FactionInvestigatorSO> _factions;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _agility;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _intelligence;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _power;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            FactionInvestigatorSO currentFaction = SetCurrent(Card.Info.Faction);
            _template.sprite = currentFaction._templateFront;
            _badge.sprite = currentFaction._badget;
            SetStats(currentFaction._stats);

            FactionInvestigatorSO SetCurrent(Faction faction) =>
               _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
               _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);
        }

        /*******************************************************************/
        private void SetStats(Sprite holderImage)
        {
            if (Card is CardInvestigator _cardInvestigator)
            {
                _health.SetStat(_cardInvestigator.Health);
                _sanity.SetStat(_cardInvestigator.Sanity);
                _strength.SetStat(_cardInvestigator.Strength, holderImage);
                _agility.SetStat(_cardInvestigator.Agility, holderImage);
                _intelligence.SetStat(_cardInvestigator.Intelligence, holderImage);
                _power.SetStat(_cardInvestigator.Power, holderImage);
            }
        }
    }
}
