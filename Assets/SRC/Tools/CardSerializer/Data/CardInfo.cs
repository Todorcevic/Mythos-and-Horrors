using GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools
{
    public class CardInfo : DataCreatorBase
    {
        [BoxGroup("Split/Left")]
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
        public string Slot { get; set; }

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
        public bool? HintFixed { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public bool? HealthPerAdventurer { get; set; }
    }
}