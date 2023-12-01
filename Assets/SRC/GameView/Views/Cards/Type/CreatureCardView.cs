using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class CreatureCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private Sprite _skillDamageIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillFearIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _skillHolder;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillIconsController _skillIconsController;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _health;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _strength;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _agility;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            if (Card.Info.CardType != CardType.Creature) return;
            SetInfo();
        }

        /*******************************************************************/
        private void SetInfo()
        {
            _health.text = Card.Info.Health.ToString();
            _strength.text = Card.Info.Strength.ToString() ?? ViewValues.EMPTY_STAT;
            _agility.text = Card.Info.Agility.ToString() ?? ViewValues.EMPTY_STAT;
            _skillIconsController.SetSkillIconView(Card.Info.EnemyDamage ?? 0, _skillDamageIcon, _skillHolder);
            _skillIconsController.SetSkillIconView(Card.Info.EnemyFear ?? 0, _skillFearIcon, _skillHolder);
        }
    }
}
