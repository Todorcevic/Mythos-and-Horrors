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
        public string Flavor { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string PackCode { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string SceneCode { get; set; }

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

        [BoxGroup("Split/Right", ShowLabel = false)]
        [SerializeField]
        public int? Health { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? Sanity { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? CreatureDamage { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int? CreatureFear { get; set; }

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
                "asset" => CardType.Supply,
                "skill" => CardType.Talent,
                "event" => CardType.Condition,
                "enemy" => CardType.Creature,
                "location" => CardType.Place,
                "treachery" => CardType.Adversity,
                "act" => CardType.Goal,
                "agenda" => CardType.Plot,
                _ => CardType.None,
            };

            Code = oldCardinfo.Code;
            Name = oldCardinfo.Name;
            Description = oldCardinfo.Text;
            Flavor = oldCardinfo.Flavor;
            PackCode = oldCardinfo.PackCode;
            SceneCode = oldCardinfo.EncounterCode;
            Faction = oldCardinfo.FactionCode switch
            {
                "survivor" => Faction.Versatile,
                "rogue" => Faction.Cunning,
                "guardian" => Faction.Brave,
                "mystic" => Faction.Esoteric,
                "seeker" => Faction.Scholarly,
                "neutral" => Faction.Neutral,
                "mythos" => Faction.Myths,
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
            Strength = oldCardinfo.SkillCombat ?? oldCardinfo.EnemyFight;
            Agility = oldCardinfo.SkillAgility ?? oldCardinfo.EnemyEvade;
            Intelligence = oldCardinfo.SkillIntellect;
            Power = oldCardinfo.SkillWillpower;
            Wild = oldCardinfo.SkillWild;
            Health = oldCardinfo.Health;
            Sanity = oldCardinfo.Sanity;
            CreatureDamage = oldCardinfo.EnemyDamage;
            CreatureFear = oldCardinfo.EnemyHorror;
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