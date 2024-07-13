//using MythosAndHorrors.GameRules;
//using Sirenix.OdinInspector;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace MythosAndHorrors.GameView
//{
//    public class CreatureCardView : CardView
//    {
//        [Title(nameof(CreatureCardView))]
//        [SerializeField, Required, AssetsOnly] private Sprite _skillDamageIcon;
//        [SerializeField, Required, AssetsOnly] private Sprite _skillFearIcon;
//        [SerializeField, Required, AssetsOnly] private Sprite _skillHolder;
//        [SerializeField, Required, ChildGameObjectsOnly] private SkillIconsController _skillIconsController;
//        [SerializeField, Required, ChildGameObjectsOnly] private MultiStatView _health;
//        [SerializeField, Required, ChildGameObjectsOnly] private StatView _strength;
//        [SerializeField, Required, ChildGameObjectsOnly] private StatView _agility;
//        [SerializeField, Required, ChildGameObjectsOnly] private StatView _eldritchableStat;
//        [SerializeField, Required, ChildGameObjectsOnly] private StatView _extraStat;

//        /*******************************************************************/
//        protected override void SetSpecific()
//        {
//            if (Card.Info.CardType != CardType.Creature) throw new Exception("Wrong CardType:" + Card.Info.CardType);
//            SetSkillIcons();
//            SetStats();
//        }

//        /*******************************************************************/
//        private void SetSkillIcons()
//        {
//            _skillIconsController.AddSkillIconView(Card.Info.CreatureDamage ?? 0, _skillDamageIcon, _skillHolder);
//            _skillIconsController.AddSkillIconView(Card.Info.CreatureFear ?? 0, _skillFearIcon, _skillHolder);
//        }

//        private void SetStats()
//        {
//            if (Card is CardCreature _creature)
//            {
//                _health.SetStat(_creature.Health);
//                _health.SetMultiStats(new List<Stat> { _creature.DamageRecived });
//                _strength.SetStat(_creature.Strength);
//                _agility.SetStat(_creature.Agility);
//            }

//            if (Card is IEldritchable eldritchable)
//            {
//                _eldritchableStat.gameObject.SetActive(true);
//                _eldritchableStat.SetStat(eldritchable.Eldritch);
//            }

//            if (Card.ExtraStat != null)
//            {
//                _extraStat.gameObject.SetActive(true);
//                _extraStat.SetStat(Card.ExtraStat);
//            }
//        }
//    }
//}
