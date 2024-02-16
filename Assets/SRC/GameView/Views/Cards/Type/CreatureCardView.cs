using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CreatureCardView : CardView
    {
        [Title(nameof(CreatureCardView))]
        [SerializeField, Required, AssetsOnly] private Sprite _skillDamageIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillFearIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillHolder;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillIconsController _skillIconsController;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _agility;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            if (Card.Info.CardType != CardType.Creature) throw new Exception("Wrong CardType:" + Card.Info.CardType);
            SetSkillIcons();
            SetStats();
        }

        /*******************************************************************/
        private void SetSkillIcons()
        {
            _skillIconsController.SetSkillIconView(Card.Info.EnemyDamage ?? 0, _skillDamageIcon, _skillHolder);
            _skillIconsController.SetSkillIconView(Card.Info.EnemyFear ?? 0, _skillFearIcon, _skillHolder);
        }

        private void SetStats()
        {
            if (Card is CardCreature _creature)
            {
                _health.SetStat(_creature.Health);
                _strength.SetStat(_creature.Strength);
                _agility.SetStat(_creature.Agility);
            }
        }
    }
}
