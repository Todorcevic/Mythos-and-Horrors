using Sirenix.OdinInspector;
using MythsAndHorrors.GameRules;
using UnityEngine;
using System.Linq;
using System;

namespace MythsAndHorrors.Tools
{
    public class CardInfo : DataCreatorBase
    {
        [BoxGroup("Split/Left")]
        [MultiLineProperty(8)]
        [SerializeField]
        public string Description { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string PackCode { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public Faction Faction { get; set; } // Versatil, Intrepid, etc.

        [BoxGroup("Split/Left")]
        [SerializeField]
        public Slot Slot { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string[] Tags { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Cost { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Quantity { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Strength { get; set; } // For adventurers card and Challenges

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Agility { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Intelligence { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Power { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public int? Wild { get; set; } // Wildcard for Challenges

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Health { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Sanity { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? EnemyDamage { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? EnemyTerror { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? EnemyStrength { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? EnemyAgility { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Xp { get; set; } //Xp need to buy the card

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Victory { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Enigma { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Hints { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Eldritch { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public bool? HealthPerAdventurer { get; set; }

        public CardInfo CreateWith(OldCardInfo oldCardinfo)
        {
            CardType = oldCardinfo.TypeCode switch
            {
                "investigator" => CardType.Adventurer,
                "asset" => CardType.Aid,
                "skill" => CardType.Talent,
                "event" => CardType.Condition,
                "enemy" => CardType.Creature,
                "location" => CardType.Place,
                "treachery" => CardType.Adversity,
                "act" => CardType.Goal,
                "plan" => CardType.Plot,
                _ => CardType.None,
            };

            Code = oldCardinfo.Code;
            Name = oldCardinfo.Name;
            Description = oldCardinfo.Text;
            PackCode = oldCardinfo.PackCode;
            Faction = oldCardinfo.FactionCode switch
            {
                "survivor" => Faction.Versatil,
                "rogue" => Faction.Intrepid,
                "guardian" => Faction.Valiant,
                "mystic" => Faction.Esoteric,
                "seeker" => Faction.Scholarly,
                _ => Faction.None,
            };

            Slot = oldCardinfo.Slot switch
            {
                "Ally" => Slot.Supporter,
                "Hand" => Slot.Item,
                "Hand x2" => Slot.Itemx2,
                "Arcane" => Slot.Magical,
                "Arcane x2" => Slot.Magicalx2,
                "Accesory" => Slot.Trinket,
                "Body" => Slot.Equipment,
                _ => Slot.None,
            };

            Tags = oldCardinfo.Traits?.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToArray();
            Cost = oldCardinfo.Cost;
            Quantity = oldCardinfo.Quantity;
            Strength = oldCardinfo.SkillCombat;
            Agility = oldCardinfo.SkillAgility;
            Intelligence = oldCardinfo.SkillIntellect;
            Power = oldCardinfo.SkillWillpower;
            Wild = oldCardinfo.SkillWild;
            Health = oldCardinfo.Health;
            Sanity = oldCardinfo.Sanity;
            EnemyDamage = oldCardinfo.EnemyDamage;
            EnemyTerror = oldCardinfo.EnemyHorror;
            EnemyStrength = oldCardinfo.EnemyFight;
            EnemyAgility = oldCardinfo.EnemyEvade;
            Xp = oldCardinfo.Xp;
            Victory = oldCardinfo.Victory;
            Enigma = oldCardinfo.Shroud;
            Hints = oldCardinfo.Clues;
            Eldritch = oldCardinfo.Doom;
            HealthPerAdventurer = oldCardinfo.HealthPerInvestigator;
            return this;
        }
    }
}