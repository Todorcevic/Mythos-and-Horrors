using Unity.Plastic.Newtonsoft.Json;

namespace MythsAndHorrors.GameRules
{
    public record CardInfo
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public CardType CardType { get; init; }
        public Faction Faction { get; init; }
        public Slot Slot { get; init; }
        public string[] Tags { get; init; }
        public int? Health { get; init; }
        public int? Sanity { get; init; }
        public int? Cost { get; init; }
        public int? Quantity { get; init; }
        public string PackCode { get; init; }
        public string SceneCode { get; init; }
        public int? Strength { get; init; }
        public int? Agility { get; init; }
        public int? Intelligence { get; init; }
        public int? Power { get; init; }
        public int? Wild { get; init; }
        [JsonIgnore] public int TotalChallengePoints => (Strength ?? 0) + (Agility ?? 0) + (Intelligence ?? 0) + (Power ?? 0) + (Wild ?? 0);
        public int? EnemyDamage { get; init; }
        public int? EnemyFear { get; init; }
        [JsonIgnore] public int TotalEnemyHits => (EnemyDamage ?? 0) + (EnemyFear ?? 0);
        public int? Xp { get; init; }
        public int? Victory { get; init; }
        public int? Enigma { get; init; }
        public int? Hints { get; init; }
        public int? Eldritch { get; init; }
        public bool? HealthPerAdventurer { get; init; }
    }
}
