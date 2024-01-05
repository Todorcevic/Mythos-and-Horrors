using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class DeckCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private List<FactionDeckSO> _factions;
        [SerializeField, Required, AssetsOnly] private Sprite _skillStrengthIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillAgilityIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillIntelligenceIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillPowerIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillWildIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceBulletIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceChargeIcon;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillIconsController _skillIconsController;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillIconsController _resourceIconsController;
        [SerializeField, Required, ChildGameObjectsOnly] private SlotController _slotController;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _titleHolder;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _costRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _healthRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _sanityRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _cost;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _sanity;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            FactionDeckSO currentFaction = SetCurrent(Card.Info.Faction);
            SetInfo();
            SetSkillPlacer(currentFaction);
            SetRenderers(currentFaction);
            SetSupporterInfo(currentFaction);
        }

        /*******************************************************************/
        public void SetBulletsIcons(int amount)
        {
            _resourceIconsController.SetSkillIconView(amount, _resourceBulletIcon, null);
        }

        public void SetChargesIcons(int amount)
        {
            _resourceIconsController.SetSkillIconView(amount, _resourceChargeIcon, null);
        }

        private FactionDeckSO SetCurrent(Faction faction) =>
            _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
            _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);

        private void SetInfo()
        {
            _cost.text = Card.Info.Cost.ToString() ?? ViewValues.EMPTY_STAT;
            _health.text = Card.Info.Health.ToString() ?? ViewValues.EMPTY_STAT;
            _sanity.text = Card.Info.Sanity.ToString() ?? ViewValues.EMPTY_STAT;
            _slotController.SetSlot(Card.Info.Slot);
        }

        private void SetSkillPlacer(FactionDeckSO currentFaction)
        {
            _skillIconsController.SetSkillIconView(Card.Info.Wild ?? 0, _skillWildIcon, currentFaction._skillHolder);
            _skillIconsController.SetSkillIconView(Card.Info.Strength ?? 0, _skillStrengthIcon, currentFaction._skillHolder);
            _skillIconsController.SetSkillIconView(Card.Info.Agility ?? 0, _skillAgilityIcon, currentFaction._skillHolder);
            _skillIconsController.SetSkillIconView(Card.Info.Intelligence ?? 0, _skillIntelligenceIcon, currentFaction._skillHolder);
            _skillIconsController.SetSkillIconView(Card.Info.Power ?? 0, _skillPowerIcon, currentFaction._skillHolder);
        }

        private void SetRenderers(FactionDeckSO currentFaction)
        {
            _template.sprite = currentFaction._templateDeckFront;
            _titleHolder.sprite = currentFaction._titleHolder;
            _badge.sprite = currentFaction._badget;
        }

        private void SetSupporterInfo(FactionDeckSO currentFaction)
        {
            _costRenderer.gameObject.SetActive(Card.Info.Cost != null);
            _healthRenderer.gameObject.SetActive(Card.Info.Health != null);
            _sanityRenderer.gameObject.SetActive(Card.Info.Sanity != null);
        }
    }
}
