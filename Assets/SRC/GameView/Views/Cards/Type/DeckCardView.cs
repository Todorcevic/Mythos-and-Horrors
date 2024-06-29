using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class DeckCardView : CardView, IUpdatable
    {
        [Title(nameof(DeckCardView))]
        [SerializeField, Required, AssetsOnly] private List<FactionDeckSO> _factions;
        [SerializeField, Required, AssetsOnly] private Sprite _skillStrengthIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillAgilityIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillIntelligenceIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillPowerIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillWildIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceBulletIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceChargeIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceSupplyIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _resourceSecretIcon;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillIconsController _skillIconsController;
        [SerializeField, Required, ChildGameObjectsOnly] private ChargesIconsController _resourceIconsController;
        [SerializeField, Required, ChildGameObjectsOnly] private SlotController _slotController;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _template;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _titleHolder;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _cost;
        [SerializeField, Required, ChildGameObjectsOnly] private MultiStatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private MultiStatView _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _extraStat;

        private bool HasCost => _cost.IsActive;
        private bool HasSlot => Card.Info.Slots.Count() > 0;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            FactionDeckSO currentFaction = SetCurrent(Card.Info.Faction);
            SetSlots();
            SetStat();
            SetSkillIcons(currentFaction);
            SetChargesIcons();
            SetRenderers(currentFaction);
        }

        /*******************************************************************/
        private void SetChargesIcons()
        {
            if (Card is not CardSupply cardSupply) return;
            if (Card is IChargeable chargeable)
            {
                switch (chargeable.Charge.ChargeType)
                {
                    case ChargeType.Bullet:
                        _resourceIconsController.IntialSet(chargeable.Charge.Amount, _resourceBulletIcon, null);
                        break;
                    case ChargeType.MagicCharge:
                        _resourceIconsController.IntialSet(chargeable.Charge.Amount, _resourceChargeIcon, null);
                        break;
                    case ChargeType.Supplie:
                        _resourceIconsController.IntialSet(chargeable.Charge.Amount, _resourceSupplyIcon, null);
                        break;
                    case ChargeType.Secret:
                        _resourceIconsController.IntialSet(chargeable.Charge.Amount, _resourceSecretIcon, null);
                        break;
                }
            }
        }

        private FactionDeckSO SetCurrent(Faction faction) =>
            _factions.Find(factionDeckSO => factionDeckSO._faction == faction) ??
            _factions.Find(factionDeckSO => factionDeckSO._faction == Faction.Neutral);

        private void SetSlots()
        {
            _slotController.SetSlots(Card.Info.Slots);
        }

        private void SetStat()
        {
            if (Card is CardSupply cardSupply)
            {
                _cost.SetStat(cardSupply.ResourceCost);
                if (cardSupply is IDamageable damageable)
                {
                    _health.SetStat(damageable.Health);
                    _health.SetMultiStats(new List<Stat> { damageable.DamageRecived });
                }
                if (cardSupply is IFearable fearable)
                {
                    _sanity.SetStat(fearable.Sanity);
                    _sanity.SetMultiStats(new List<Stat> { fearable.FearRecived });
                }
            }
            else if (Card is CardCondition cardCondition)
            {
                _cost.SetStat(cardCondition.ResourceCost);
            }

            if (Card.ExtraStat != null)
            {
                _extraStat.gameObject.SetActive(true);
                _extraStat.SetStat(Card.ExtraStat);
            }
        }

        private void SetSkillIcons(FactionDeckSO currentFaction)
        {
            _skillIconsController.AddSkillIconView(Card.Info.Wild ?? 0, _skillWildIcon, currentFaction._skillHolder);
            _skillIconsController.AddSkillIconView(Card.Info.Strength ?? 0, _skillStrengthIcon, currentFaction._skillHolder);
            _skillIconsController.AddSkillIconView(Card.Info.Agility ?? 0, _skillAgilityIcon, currentFaction._skillHolder);
            _skillIconsController.AddSkillIconView(Card.Info.Intelligence ?? 0, _skillIntelligenceIcon, currentFaction._skillHolder);
            _skillIconsController.AddSkillIconView(Card.Info.Power ?? 0, _skillPowerIcon, currentFaction._skillHolder);
        }

        private void SetRenderers(FactionDeckSO currentFaction)
        {
            _template.sprite = currentFaction._templateDeckFront;
            _titleHolder.sprite = currentFaction._titleHolder;
            _badge.sprite = currentFaction._badget;
        }

        /*******************************************************************/
        void IUpdatable.Show()
        {
            ChangeColorResource();
            ChangeSlotColor();
        }

        void IUpdatable.Hide()
        {
            _cost.Default();
            _slotController.Default();
        }

        private void ChangeColorResource()
        {
            if (!HasCost) return;
            if (Card.CurrentZone != Card.Owner?.HandZone) _cost.Default();
            else if (_cost.Stat.Value > Card.Owner?.Resources.Value) _cost.Deactive();
            else _cost.Active();
        }

        private void ChangeSlotColor()
        {
            if (!HasSlot) return;
            if (Card.CurrentZone == Card.Owner?.AidZone) _slotController.Active(2);
            else if (Card.CurrentZone != Card.Owner?.HandZone) _slotController.Default();
            else _slotController.Active(Card.Info.Slots.Count());
        }
    }
}
