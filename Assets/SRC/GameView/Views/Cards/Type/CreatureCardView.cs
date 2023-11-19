using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CreatureCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private Sprite _skillDamageIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillFearIcon;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SkillIconView> _skillPlacer;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _agility;

        /*******************************************************************/
        protected override void SetAll()
        {
            if (Card.Info.CardType != CardType.Creature) return;
            SetInfo();
            SetSkillPlacer();
        }

        /*******************************************************************/
        private void SetInfo()
        {
            _health.text = Card.Info.Health.ToString();
            _strength.text = Card.Info.Strength.ToString() ?? ViewValues.EMPTY_STAT;
            _agility.text = Card.Info.Agility.ToString() ?? ViewValues.EMPTY_STAT;
        }

        private void SetSkillPlacer()
        {

            for (int i = 0; i < Card.Info.EnemyDamage; i++) GetNexPlacertInactive().SetSkillIcon(_skillDamageIcon);
            for (int i = 0; i < Card.Info.EnemyFear; i++) GetNexPlacertInactive().SetSkillIcon(_skillFearIcon);

        }

        private SkillIconView GetNexPlacertInactive() => _skillPlacer.Find(x => x.IsInactive);
    }
}
