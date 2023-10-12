using Codice.Client.Common;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CardFrontView : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private FactionAdventurerSO _versatile;
        [SerializeField, AssetsOnly] private FactionAdventurerSO _neutral;
        [SerializeField, AssetsOnly] private FactionAdventurerSO _cunning;
        [SerializeField, AssetsOnly] private FactionAdventurerSO _brave;
        [SerializeField, AssetsOnly] private FactionAdventurerSO _scholarly;
        [SerializeField, AssetsOnly] private FactionAdventurerSO _esoteric;

        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField] private SpriteRenderer _picture;
        [SerializeField] private SpriteRenderer _badge;
        [SerializeField] private SpriteRenderer _health;
        [SerializeField] private SpriteRenderer _sanity;
        [SerializeField] private List<SpriteRenderer> _stats;
        [SerializeField] private SpriteRenderer _templateDeckFront;
        [SerializeField] private SpriteRenderer _cost;
        [SerializeField] private List<SpriteRenderer> _skillPlacer;

        /*******************************************************************/

        public void SetFront(Card thisCard)
        {
            FactionAdventurerSO currentFaction = SetCurrent(thisCard.Info.Faction);
            if (currentFaction == null) return;

            _template.sprite = thisCard.IsScenaryCard ? _template.sprite : currentFaction._templateFront;

            if (_badge != null) _badge.sprite = currentFaction._badget;
            if (_health != null) _health.sprite = currentFaction._health;
            if (_sanity != null) _sanity.sprite = currentFaction._sanity;
            if (_stats.Count > 0) _stats.ForEach(spriteRenderer => spriteRenderer.sprite = currentFaction._stats);
            //if (_cost != null) _cost.sprite = currentFaction._cost;
            //if (_skillPlacer.Count > 0) _skillPlacer.ForEach(spriteRenderer => spriteRenderer.sprite = currentFaction._assistant);
        }

        private Sprite GetPicture(Card thisCard)
        {
            throw new NotImplementedException();
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
                _ => null,
            };
        }
    }
}
