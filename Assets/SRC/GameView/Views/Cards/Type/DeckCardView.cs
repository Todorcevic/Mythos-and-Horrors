using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class DeckCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private FactionDeckSO _versatile;
        [SerializeField, Required, AssetsOnly] private FactionDeckSO _cunning;
        [SerializeField, Required, AssetsOnly] private FactionDeckSO _brave;
        [SerializeField, Required, AssetsOnly] private FactionDeckSO _scholarly;
        [SerializeField, Required, AssetsOnly] private FactionDeckSO _esoteric;

        [SerializeField, Required, AssetsOnly] private Sprite _skillStrengthIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillAgilityIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillIntelligenceIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillPowerIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillWildIcon;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SkillIconView> _skillPlacer;

        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _costRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _healthRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _sanityRenderer;

        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _cost;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _sanity;

        /*******************************************************************/
        protected override void SetAll()
        {
            FactionDeckSO currentFaction = SetCurrent(Card.Info.Faction);
            SetSkillPlacer();
            SetInfo();
            if (currentFaction == null) return;
            SetRenderers(currentFaction);
            SetBadget(currentFaction);
            SetSupporterInfo(currentFaction);
        }

        /*******************************************************************/
        private void SetSkillPlacer()
        {
            for (int i = 0; i < Card.Info.Wild; i++) GetNextPlacerInactive().SetSkillIcon(_skillWildIcon);
            for (int i = 0; i < Card.Info.Strength; i++) GetNextPlacerInactive().SetSkillIcon(_skillStrengthIcon);
            for (int i = 0; i < Card.Info.Agility; i++) GetNextPlacerInactive().SetSkillIcon(_skillAgilityIcon);
            for (int i = 0; i < Card.Info.Intelligence; i++) GetNextPlacerInactive().SetSkillIcon(_skillIntelligenceIcon);
            for (int i = 0; i < Card.Info.Power; i++) GetNextPlacerInactive().SetSkillIcon(_skillPowerIcon);
        }

        private void SetInfo()
        {
            _cost.text = Card.Info.Cost.ToString();
            _health.text = Card.Info.Health.ToString();
            _sanity.text = Card.Info.Sanity.ToString();
        }

        private void SetRenderers(FactionDeckSO currentFaction)
        {
            _template.sprite = currentFaction._templateDeckFront;
            _costRenderer.sprite = currentFaction._cost;
            _skillPlacer.ForEach(spriteRenderer => spriteRenderer.SetHolder(currentFaction._skillHolder));
        }

        private void SetSupporterInfo(FactionDeckSO currentFaction)
        {
            _healthRenderer.gameObject.SetActive(Card.Info.Health != null);
            _sanityRenderer.gameObject.SetActive(Card.Info.Sanity != null);
            _healthRenderer.sprite = _sanityRenderer.sprite = currentFaction._supporter;
        }

        private void SetBadget(FactionDeckSO currentFaction)
        {
            _badge.gameObject.SetActive(true);
            _badge.sprite = currentFaction._badget;
        }

        private FactionDeckSO SetCurrent(Faction faction)
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

        private SkillIconView GetNextPlacerInactive() => _skillPlacer.Find(x => x.IsInactive);
    }
}
