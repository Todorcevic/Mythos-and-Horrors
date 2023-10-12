using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class SkillPlacerView : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private Sprite _skillStrengthIcon;
        [SerializeField, AssetsOnly] private Sprite _skillAgilityIcon;
        [SerializeField, AssetsOnly] private Sprite _skillIntelligenceIcon;
        [SerializeField, AssetsOnly] private Sprite _skillPowerIcon;
        [SerializeField, AssetsOnly] private Sprite _skillWildIcon;
        [SerializeField, AssetsOnly] private Sprite _skillDamageIcon;
        [SerializeField, AssetsOnly] private Sprite _skillFearIcon;
        [SerializeField, ChildGameObjectsOnly] private List<SkillIconView> _skillPlacer;

        /*******************************************************************/
        public void SetSkillPlacer(Card thisCard)
        {
            Debug.Log("Doing...: " + thisCard.Info.Code);


            if (thisCard.Info.CardType != CardType.Creature)
            {
                for (int i = 0; i < thisCard.Info.Strength; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillStrengthIcon);
                }

                for (int i = 0; i < thisCard.Info.Agility; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillAgilityIcon);
                }

                for (int i = 0; i < thisCard.Info.Intelligence; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillIntelligenceIcon);
                }

                for (int i = 0; i < thisCard.Info.Power; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillPowerIcon);
                }

                for (int i = 0; i < thisCard.Info.Wild; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillWildIcon);
                }
            }
            else
            {
                for (int i = 0; i < thisCard.Info.EnemyDamage; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillDamageIcon);
                }

                for (int i = 0; i < thisCard.Info.EnemyFear; i++)
                {
                    GetNexPlacertInactive().SetSkillIcon(_skillFearIcon);
                }
            }
        }

        private SkillIconView GetNexPlacertInactive() => _skillPlacer.Find(x => x.IsInactive);
    }
}
