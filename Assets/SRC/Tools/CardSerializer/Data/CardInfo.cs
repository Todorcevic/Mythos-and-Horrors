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
        public string Description2 { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string Flavor { get; set; }
        [BoxGroup("Split/Left")]
        [SerializeField]
        public string Flavor2 { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string PackCode { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string Name2 { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string SceneCode { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public Faction Faction { get; set; } // Versatil, Intrepid, etc.

        [BoxGroup("Split/Left")]
        [SerializeField]
        public SlotType[] Slots { get; set; }

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
        public bool? HealthPerInvestigator { get; set; }

        public CardInfo CreateWith(OldCardInfo oldCardinfo)
        {
            CardType = oldCardinfo.TypeCode switch
            {
                "investigator" => CardType.Investigator,
                "asset" => CardType.Supply,
                "skill" => CardType.Talent,
                "event" => CardType.Condition,
                "enemy" => CardType.Creature,
                "location" => CardType.Place,
                "treachery" => CardType.Adversity,
                "act" => CardType.Goal,
                "agenda" => CardType.Plot,
                "scenario" => CardType.Scene,
                _ => CardType.None,
            };

            Code = oldCardinfo.Code;
            Name = oldCardinfo.Name;
            Name2 = oldCardinfo.BackName;
            Description = oldCardinfo.Text;
            Description2 = oldCardinfo.BackText;
            Flavor = oldCardinfo.Flavor;
            Flavor2 = oldCardinfo.BackFlavor;
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

            Slots = oldCardinfo.Slot switch
            {
                "Ally" => new[] { SlotType.Supporter },
                "Hand" => new[] { SlotType.Item },
                "Hand x2" => new[] { SlotType.Item, SlotType.Item },
                "Arcane" => new[] { SlotType.Magical },
                "Arcane x2" => new[] { SlotType.Magical, SlotType.Magical },
                "Accessory" => new[] { SlotType.Trinket },
                "Body" => new[] { SlotType.Equipment },
                _ => new[] { SlotType.None },
            };

            var traits = oldCardinfo.Traits?.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()) ?? Enumerable.Empty<string>();
            var subtypeName = oldCardinfo.SubtypeName != null ? new[] { oldCardinfo.SubtypeName } : Enumerable.Empty<string>();
            Tags = new[] { Enum.GetName(typeof(CardType), CardType) }.Concat(subtypeName).Concat(traits).ToArray();
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
            HealthPerInvestigator = oldCardinfo.HealthPerInvestigator;
            return this;
        }
    }
}