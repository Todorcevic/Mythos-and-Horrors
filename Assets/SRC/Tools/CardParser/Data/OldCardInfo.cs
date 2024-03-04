// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace MythosAndHorrors.Tools
{
    public class DeckOption
    {
        [SerializeField]
        [JsonProperty("faction")]
        public List<string> Faction { get; set; }

        [SerializeField]
        [JsonProperty("level")]
        public Level Level { get; set; }
    }

    public class DeckRequirements
    {
        [SerializeField]
        [JsonProperty("size")]
        public int? Size { get; set; }

        [SerializeField]
        [JsonProperty("card")]
        public List<string> Card { get; set; }

        [SerializeField]
        [JsonProperty("random")]
        public List<Random> Random { get; set; }
    }

    public class ErrataDate
    {
        [SerializeField]
        [JsonProperty("date")]
        public string Date { get; set; }

        [SerializeField]
        [JsonProperty("timezone_type")]
        public int? TimezoneType { get; set; }

        [SerializeField]
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

    public class Level
    {
        [SerializeField]
        [JsonProperty("min")]
        public int? Min { get; set; }

        [SerializeField]
        [JsonProperty("max")]
        public int? Max { get; set; }
    }

    public class Random
    {
        [SerializeField]
        [JsonProperty("target")]
        public string Target { get; set; }

        [SerializeField]
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Restrictions
    {
        [SerializeField]
        [JsonProperty("investigator")]
        public string Investigator { get; set; }

        [SerializeField]
        [JsonProperty("investigators")]
        public List<string> Investigators { get; set; }
    }

    public class OldCardInfo
    {
        [HorizontalGroup("Split", 0.33f)]
        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("code")]
        public string Code { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("pack_code")]
        public string PackCode { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("pack_name")]
        public string PackName { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("type_code")]
        public string TypeCode { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("type_name")]
        public string TypeName { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("faction_code")]
        public string FactionCode { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("faction_name")]
        public string FactionName { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("subname")]
        public string Subname { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("text")]
        public string Text { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("skill_willpower")]
        public int? SkillWillpower { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("skill_intellect")]
        public int? SkillIntellect { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("skill_combat")]
        public int? SkillCombat { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        [JsonProperty("skill_agility")]
        public int? SkillAgility { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("health")]
        public int? Health { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("health_per_investigator")]
        public bool? HealthPerInvestigator { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("sanity")]
        public int? Sanity { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("deck_limit")]
        public int? DeckLimit { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("slot")]
        public string Slot { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("traits")]
        public string Traits { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("flavor")]
        public string Flavor { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("back_text")]
        public string BackText { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("back_flavor")]
        public string BackFlavor { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("cost")]
        public int? Cost { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("skill_wild")]
        public int? SkillWild { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("subtype_code")]
        public string SubtypeCode { get; set; }

        [BoxGroup("Split/center")]
        [SerializeField]
        [JsonProperty("subtype_name")]
        public string SubtypeName { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("xp")]
        public int? Xp { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("enemy_damage")]
        public int? EnemyDamage { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("enemy_horror")]
        public int? EnemyHorror { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("enemy_fight")]
        public int? EnemyFight { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("enemy_evade")]
        public int? EnemyEvade { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("encounter_code")]
        public string EncounterCode { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("encounter_name")]
        public string EncounterName { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("doom")]
        public int? Doom { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("stage")]
        public int? Stage { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("back_name")]
        public string BackName { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("clues")]
        public int? Clues { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("shroud")]
        public int? Shroud { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("victory")]
        public int? Victory { get; set; }

        //[BoxGroup("Split/right")]
        //[SerializeField]
        //[JsonProperty("linked_card")]
        //public OldCardInfo LinkedCard { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("imagesrc")]
        public string Image { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("position")]
        public int? Position { get; set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("is_complete")]
        public bool IsComplete { get; set; }

        public bool Contains(string word)
        {
            return Code.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Name.Contains(word, StringComparison.OrdinalIgnoreCase)
                || TypeCode.Contains(word, StringComparison.OrdinalIgnoreCase);
        }
    }
}